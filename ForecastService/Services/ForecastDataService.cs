using ForecastService.Contracts;
using ForecastService.Models.OpenMeteo.Responses;
using ForecastService.Models.Pollinator;
using ForecastService.Models.Pollinator.DAOs;
using ForecastService.Models.Pollinator.DTOs;
using ForecastService.Models.Pollinator.QueryParams;
using ForecastService.Models.Pollinator.Settings;
using ForecastService.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Events;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using YpenService.Contracts;
using YpenService.Models.Pollinator.Business;
using ForecastService.Mapping;
using AutoMapper;

namespace ForecastService.Services
{
    public class ForecastDataService(
        ILogger<ForecastDataService> _logger,
        IOptions<OpenMeteoSettings> openMeteoSettings,
        IForecastClient client,
        IYpenService _ypenService,
        IForecastRepository _repository,
        IMapper _mapper) : IForecastDataService
    {
        private readonly OpenMeteoSettings _openMeteoSettings = openMeteoSettings.Value;

        #region Init services
        public async Task<ServiceResponse<List<AirQualityResponse>>> LoadAirQualForecast(List<AirQualityIndicator> indexes)
        {
            string method = "LoadAirQualForecast";
            _logger.LogInformation($"IN Method {method} called requesting {indexes.Count} parameters");
            try
            {
                AirQualityParams airQualityParams = new AirQualityParams()
                {
                    HourlyAirParams = indexes
                };
                List<RegionCenterDto> centers = await ExtractCenters(airQualityParams);
                var requestUri = CreateQueryUrl(airQualityParams, openMeteoSettings.Value.AirQualityBaseUrl);
                var resp = await client.GetAsync<List<AirQualityResponse>>(requestUri);
                if (resp.Count != centers.Count)
                    throw new Exception($"Open-Meteo returned {resp.Count} results but {centers.Count} centers were requested — cannot reliably match Kalcode.");
                //TODO this should be removed when finalized
                _logger.LogInformation("OUT Method {method} got a response: {response}", method, JsonSerializer.Serialize(resp));
                var airquality = AirQualityToCentersMerger(centers, resp);
                await _repository.UpdateAirQualityAsync(airquality);
                return new ServiceResponse<List<AirQualityResponse>>(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR Method {method} failed with exception: {ex?.Message ?? "Unknown exception!"}");
                throw;
            }
        }

        private List<AirQualityDAO> AirQualityToCentersMerger(List<RegionCenterDto> centers, List<AirQualityResponse> airQuality)
        {
            var result = new List<AirQualityDAO>();
            for (int i = 0; i < centers.Count; i++)
            {
                var center = centers[i];
                var hourlyData = airQuality[i].hourly;
                if (Math.Abs(center.Latitude - airQuality[i].latitude) > 0.1 ||
                    Math.Abs(center.Longitude - airQuality[i].longitude) > 0.1)
                    throw new Exception($"Mismatch between center coordinates and air quality response for Kalcode {center.KALCODE}. Center: ({center.Latitude}, {center.Longitude}), Weather: ({airQuality[i].latitude}, {airQuality[i].longitude})");

                var days = hourlyData.time
                    .Select((t, idx) => (Date: DateOnly.Parse(t), Index: idx))
                    .GroupBy(x => x.Date);

                foreach (var day in days)
                {
                    result.Add(new AirQualityDAO
                    {
                        Kalcode = center.KALCODE,
                        Time = day.Key,
                        Dust = day.Average(x => hourlyData.dust![x.Index] ?? 0),
                        AlderPollen = day.Average(x => hourlyData.alder_pollen![x.Index] ?? 0),
                        BirchPollen = day.Average(x => hourlyData.birch_pollen![x.Index] ?? 0),
                        GrassPollen = day.Average(x => hourlyData.grass_pollen![x.Index] ?? 0),
                        MugwortPollen = day.Average(x => hourlyData.mugwort_pollen![x.Index] ?? 0),
                        OlivePollen = day.Average(x => hourlyData.olive_pollen![x.Index] ?? 0),
                        RagweedPollen = day.Average(x => hourlyData.ragweed_pollen![x.Index] ?? 0),
                        PM10 = day.Average(x => hourlyData.pm10![x.Index] ?? 0),
                        PM2_5 = day.Average(x => hourlyData.pm2_5![x.Index] ?? 0),
                        AQI = day.Average(x => hourlyData.european_aqi![x.Index] ?? 0),
                        O3 = day.Average(x => hourlyData.ozone![x.Index] ?? 0),
                        NO2 = day.Average(x => hourlyData.nitrogen_dioxide![x.Index] ?? 0)
                    });
                }
            }
            return result;
            //throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<WeatherDTO>>> LoadWeatherForecast(List<WeatherIndicator> indexes)
        {
            string method = "LoadWeatherForecast";
            _logger.LogInformation("IN Method {method} called requesting {count} indexes", method, indexes.Count);
            try
            {
                WeatherParams weatherParamsList = new WeatherParams()
                {
                    DailyWeatherIndicators = indexes
                };
                List<RegionCenterDto> centers = await ExtractCenters(weatherParamsList);
                var requestUri = CreateQueryUrl(weatherParamsList, openMeteoSettings.Value.WeatherBaseUrl);
                var resp = await client.GetAsync<List<WeatherForecastResponse>>(requestUri);
                if (resp.Count != centers.Count)
                    throw new Exception($"Open-Meteo returned {resp.Count} results but {centers.Count} centers were requested — cannot reliably match Kalcode.");
                var weather = WeatherToCentersMerger(centers, resp);
                await _repository.UpdateWeatherAsync(weather);
                _logger.LogInformation("OUT Method {method} got a response: {response}", method, JsonSerializer.Serialize(resp));
                return new ServiceResponse<List<WeatherDTO>>(_mapper.Map<List<WeatherDTO>>(weather));
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR Method {method} failed with exception: {ex?.Message ?? "Unknown exception!"}");
                throw;
            }

        }
        #endregion


        #region Forecast Services


        #region AirQuality Services
        public async Task<List<AirQualityDTO>> GetTotalAirQuality4D()
        {
            string method = "GetTotalAirQuality4D";
            _logger.LogInformation($"IN Method {method} called");
            AirQualityParams airQualityParamsList = new();
            List<RegionCenterDto> centers = await ExtractCenters(airQualityParamsList);
            List<AirQualityDAO> airData = await _repository.GetAirQualityAsync(centers);
            _logger.LogInformation($"OUT Method {method} returning {airData.Count} records");
            return _mapper.Map<List<AirQualityDTO>>(airData);
        }
        public async Task<List<AirQualityDTO>> GetAirQuality4DById(string kalcode)
        {
            string method = "GetAirQuality4DById";
            _logger.LogInformation($"IN Method {method} called with kalcode: {kalcode}");
            AirQualityParams airParamsList = new();
            List<RegionCenterDto> centers = await ExtractCenter(airParamsList, kalcode);
            List<AirQualityDAO> airData = await _repository.GetAirQualityAsync(centers);
            _logger.LogInformation($"OUT Method {method} returning {airData.Count} record for kalcode: {kalcode}");
            return _mapper.Map<List<AirQualityDTO>>(airData);
        }
        #endregion

        #region Weather Services
        public async Task<List<WeatherDTO>> GetTotalWeather4D()
        {
            string method = "GetTotalWeather4D";
            _logger.LogInformation($"IN Method {method} called");
            WeatherParams weatherParamsList = new();
            List<RegionCenterDto> centers = await ExtractCenters(weatherParamsList);
            List<WeatherDAO> weatherData = await _repository.GetWeatherAsync(centers);
            _logger.LogInformation($"OUT Method {method} returning {weatherData.Count} records");
            return _mapper.Map<List<WeatherDTO>>(weatherData);
        }

        public async Task<List<WeatherDTO>> GetWeather4DById(string kalcode)
        {
            string method = "GetWeather4DById";
            _logger.LogInformation($"IN Method {method} called with kalcode: {kalcode}");
            WeatherParams weatherParamsList = new();
            List<RegionCenterDto> centers = await ExtractCenter(weatherParamsList, kalcode);
            List<WeatherDAO> weatherData = await _repository.GetWeatherAsync(centers);
            _logger.LogInformation($"OUT Method {method} returning {weatherData.Count} record for kalcode: {kalcode}");
            return _mapper.Map<List<WeatherDTO>>(weatherData);
        }
        #endregion

        #endregion


        #region Helpers
        private List<WeatherDAO> WeatherToCentersMerger(List<RegionCenterDto> centers, List<WeatherForecastResponse> weather)
        {
            List<WeatherDAO> weatherDTOs = new List<WeatherDAO>();
            for (int i = 0; i < centers.Count; i++)
            {
                if (Math.Abs(centers[i].Latitude - weather[i].latitude) > 0.1 ||
                    Math.Abs(centers[i].Longitude - weather[i].longitude) > 0.1 )
                    throw new Exception($"Mismatch between center coordinates and weather response for Kalcode {centers[i].KALCODE}. Center: ({centers[i].Latitude}, {centers[i].Longitude}), Weather: ({weather[i].latitude}, {weather[i].longitude})");
                for (int j = 0; j < weather[i].daily.time.Count; j++)
                {
                    weatherDTOs.Add(new WeatherDAO
                    {
                        Kalcode = centers[i].KALCODE,
                        WmoCode = weather[i].daily.weather_code[j],
                        TemperatureMax = weather[i].daily.temperature_2m_max[j],
                        TemperatureMin = weather[i].daily.temperature_2m_min[j],
                        WindSpeed = weather[i].daily.wind_speed_10m_max[j],
                        PrecipitationPct = weather[i].daily.precipitation_probability_max[j],
                        Humidity = weather[i].daily.relative_humidity_2m_max[j],
                        Time = DateOnly.Parse(weather[i].daily.time[j])
                    });
                }
            }
            return weatherDTOs;
        }


        private async Task<List<RegionCenterDto>> ExtractCenters(OpenMeteoParams paramsType)
        {
            _logger.LogInformation($"Acquiring region centers for forecast data request");
            List<RegionCenterDto> centers = (await _ypenService.GetCenters()).OrderBy(c => c.KALCODE).ToList();
            if (!centers.Any())
                throw new Exception("No region centers found.");
            paramsType.Latitudes.Clear();
            paramsType.Longitudes.Clear();
            foreach (RegionCenterDto center in centers)
            {
                paramsType.Latitudes.Add((float)center.Latitude);
                paramsType.Longitudes.Add((float)center.Longitude);
            }
            _logger.LogInformation($"{centers.Count} region centers successfully acquired");
            return centers;
        }

        private async Task<List<RegionCenterDto>> ExtractCenter(OpenMeteoParams paramsType, string kalcode)
        {
            _logger.LogInformation($"Acquiring region center for forecast data request");
            RegionCenterDto center = await _ypenService.GetCenter(kalcode);
            if (center is null)
                throw new Exception($"No region center found with kalcode: {kalcode}.");
            paramsType.Latitudes.Add(center.Latitude);
            paramsType.Longitudes.Add(center.Longitude);
            _logger.LogInformation($"Region center with kalcode:{kalcode} successfully acquired");
            return new List<RegionCenterDto> { center };
        }

        private string CreateQueryUrl<TRequest>(TRequest queryParams, string url)
        {
            _logger.LogInformation($"Creating request with URL: {url} and query parameters: {queryParams}");
            var query = new QueryString();

            foreach (var property in typeof(TRequest).GetProperties())
            {
                var value = property.GetValue(queryParams);
                if (value is null)
                    continue;
                var jsonAttr = property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? property.Name;
                if (value is System.Collections.IEnumerable enumerable)
                {
                    var items = enumerable.Cast<object>().Select(i => i.ToString());
                    query = query.Add(jsonAttr, string.Join(",", items));
                    continue;
                }
                query = query.Add(jsonAttr, value.ToString());
            }

            if (string.IsNullOrEmpty(query.Value))
                throw new ArgumentException("No valid query parameters provided.");
            _logger.LogInformation($"Request url created successfully: {url} + {query.Value}");

            return url + query.Value.TrimStart('?');
        }
        #endregion
    }
}

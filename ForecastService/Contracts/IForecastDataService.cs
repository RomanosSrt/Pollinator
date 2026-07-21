using ForecastService.Models.OpenMeteo.Responses;
using ForecastService.Models.Pollinator;
using ForecastService.Models.Pollinator.DTOs;
using ForecastService.Models.Pollinator.QueryParams;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Contracts
{
    public interface IForecastDataService
    {
        #region AirQuality Services
        Task<List<AirQualityDTO>> GetTotalAirQuality4D();
        Task<AirQualityDTO> GetAirQuality4DById(string kalcode);
        #endregion

        #region Weather Services
        Task<List<WeatherDTO>> GetTotalWeather4D();
        Task<WeatherDTO> GetWeather4DById(string kalcode);
        #endregion

        #region Init Services
        Task<ServiceResponse<List<AirQualityResponse>>> Get5DAirQualForecast(AirQualityParams queryParams);
        Task<ServiceResponse<List<WeatherDTO>>> Get5DWeatherForecast(List<WeatherIndicator> indexes);
        #endregion
    }
}

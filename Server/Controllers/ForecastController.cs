using API.Middleware;
using ForecastService.Contracts;
using ForecastService.Models.OpenMeteo.Responses;
using ForecastService.Models.Pollinator;
using ForecastService.Models.Pollinator.DTOs;
using ForecastService.Models.Pollinator.QueryParams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ForecastController : ControllerBase
    {
        private readonly ILogger<ForecastController> _logger;
        private readonly IForecastDataService _forecastService;

        public ForecastController(ILogger<ForecastController> logger,  IForecastDataService forecastService)
        {
            _logger = logger;
            _forecastService = forecastService;
        }

        #region Init controllers
        [Obsolete]
        [HttpGet("loadAirQualityIndexes")]
        [ProducesResponseType(typeof(ApiResponse<ServiceResponse<List<AirQualityResponse>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ServiceResponse<List<AirQualityResponse>>> Load5DAirForecast()
        {
            _logger.LogInformation($"Requesting air quality indexes");
            var airData = await _forecastService.LoadAirQualForecast();
            return airData;
        }

        [Obsolete]
        [HttpGet("loadWeatherIndexes")]
        [ProducesResponseType(typeof(ApiResponse<ServiceResponse<List<WeatherForecastResponse>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ServiceResponse<List<WeatherDTO>>> Load5DWeatherForecast()
        {
            _logger.LogInformation($"Requesting weather indexes");
            var weatherData = await _forecastService.LoadWeatherForecast();
            return weatherData;
        }


        [HttpGet("checkTime")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<bool> CheckTime()
        {
            return await _forecastService.CheckDBData();
        }
        #endregion


        #region Air Quality Controllers
        [HttpGet("airQualityIndexes")]
        [ProducesResponseType(typeof(ApiResponse<List<AirQualityDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<List<AirQualityDTO>> AirForecast()
        {
            _logger.LogInformation($"Requesting air quality indexes");
            var airData = await _forecastService.GetTotalAirQuality4D();
            return airData;
        }

        [HttpGet("airQualityIndex/{kalcode}")]
        [ProducesResponseType(typeof(ApiResponse<List<AirQualityDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<List<AirQualityDTO>> AirForecast(string kalcode)
        {
            _logger.LogInformation($"Requesting air quality indexes");
            var airData = await _forecastService.GetAirQuality4DById(kalcode);
            return airData;
        }
        #endregion


        #region Weather Controllers
        [HttpGet("weatherIndexes")]
        [ProducesResponseType(typeof(ApiResponse<List<WeatherDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<List<WeatherDTO>> WeatherForecast()
        {
            _logger.LogInformation($"Requesting air quality indexes");
            var weatherData = await _forecastService.GetTotalWeather4D();
            return weatherData;
        }

        [HttpGet("weatherIndex/{kalcode}")]
        [ProducesResponseType(typeof(ApiResponse<WeatherDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<List<WeatherDTO>> WeatherForecast(string kalcode)
        {
            _logger.LogInformation($"Requesting air quality indexes");
            var weatherData = await _forecastService.GetWeather4DById(kalcode);
            return weatherData;
        }
        #endregion
    }
}

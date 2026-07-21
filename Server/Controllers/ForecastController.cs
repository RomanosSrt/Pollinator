using API.Middleware;
using ForecastService.Contracts;
using ForecastService.Models.OpenMeteo.Responses;
using ForecastService.Models.Pollinator;
using ForecastService.Models.Pollinator.DTOs;
using ForecastService.Models.Pollinator.QueryParams;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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

        //[Authorize]
        [HttpGet("AirQualityIndexes")]
        [ProducesResponseType(typeof(ApiResponse<ServiceResponse<List<AirQualityResponse>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ServiceResponse<List<AirQualityResponse>>> Load5DAirForecast([FromQuery] AirQualityParams airParams)
        {
            _logger.LogInformation($"Requesting air quality indexes using parameters: {airParams}");
            var airData = await _forecastService.Get5DAirQualForecast(airParams);
            return airData;
        }

        [HttpGet("WeatherIndexes")]
        [ProducesResponseType(typeof(ApiResponse<ServiceResponse<List<WeatherForecastResponse>>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ServiceResponse<List<WeatherDTO>>> Load5DWeatherForecast([FromQuery] List<WeatherIndicator> indexes)
        {
            _logger.LogInformation($"Requesting weather indexes using parameters: {indexes}");
            var weatherData = await _forecastService.Get5DWeatherForecast(indexes);
            return weatherData;
        }
    }
}

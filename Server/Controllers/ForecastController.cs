using API.Application.DTOs.UserInsertion;
using API.Application.Services.Interfaces;
using API.Middleware;
using ForecastService.Contracts;
using ForecastService.Models.OpenMeteo.Responses;
using ForecastService.Models.Pollinator.QueryParams;
using ForecastService.Services;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("PollenIndexes")]
        [ProducesResponseType(typeof(ApiResponse<ServiceResponse<PollenIndexesResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<ServiceResponse<PollenIndexesResponse>> RegisterUser([FromQuery] PollenIndexesParams pollenParams)
        {
            _logger.LogInformation($"Requesting pollen indexes using parameters: {pollenParams}");
            var pollenData = await _forecastService.Get3DPollenIndexes(pollenParams);
            return pollenData;
        }
    }
}

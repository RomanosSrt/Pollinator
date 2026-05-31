using API.Middleware;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using YpenService.Contracts;
using YpenService.Models.Pollinator;
using YpenService.Models.Pollinator.Persistence;
using System.Linq;
using AutoMapper.Internal;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YpenController : ControllerBase
    {
        private readonly ILogger<YpenController> _logger;
        private readonly IYpenService _ypenService;

        public YpenController(ILogger<YpenController> logger,  IYpenService ypenService)
        {
            _logger = logger;
            _ypenService = ypenService;
        }

        //[Authorize]
        [HttpGet("RegionData")]
        [ProducesResponseType(typeof(ApiResponse<List<RegionUnitsDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<List<RegionUnitsDto>> GetRegionData()
        {
            _logger.LogInformation($"Requesting Greek regional unit shapes and centers from YPEN");
            var centers = await _ypenService.GetRegionCenters();
            var units = await _ypenService.GetRegionUnits();
            if (!centers.IsSuccess || !units.IsSuccess)
            {
                string error = centers.ErrorMessage ?? units.ErrorMessage ?? "Unknown Error on Units and Centers request";
                _logger.LogError($"Failed to retrieve region centers from YPEN: {error}");
                throw new Exception(error);
            }
            //var persistResp = await _ypenService.PersistUnits(centers.Response!, units.Response!);
            return units.Response;
        }
    }
}

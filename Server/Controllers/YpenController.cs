using API.Middleware;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using YpenService.Contracts;
using YpenService.Models.Pollinator;
using YpenService.Models.Pollinator.Persistence;
using System.Linq;
using AutoMapper.Internal;
using YpenService.Models.Pollinator.Business;

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



        #region Units Endpoints
        //[Authorize]
        [HttpGet("RegionUnitsData")]
        [ProducesResponseType(typeof(ApiResponse<List<RegionUnitDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<List<RegionUnitDto>?> GetRegionUnitsData()
        {
            _logger.LogInformation($"Requesting Greek regional unit shapes");
            var units = await _ypenService.GetUnits();
            //var units = await _ypenService.GetYpenRegionUnits();
            return units;
        }

        [HttpGet("RegionUnitData/{KALCODE}")]
        [ProducesResponseType(typeof(ApiResponse<RegionUnitDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<RegionUnitDto?> GetRegionUnitData([FromRoute] string KALCODE)
        {
            _logger.LogInformation($"Requesting Greek regional unit shape for id: {KALCODE}");
            var unit = await _ypenService.GetUnit(KALCODE);
            //var units = await _ypenService.GetYpenRegionUnits();
            return unit;
        }
        #endregion

        #region Centers Endpoints
        //[Authorize]
        [HttpGet("RegionCentersData")]
        [ProducesResponseType(typeof(ApiResponse<List<RegionCenterDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<List<RegionCenterDto>?> GetRegionCentersData()
        {
            _logger.LogInformation($"Requesting Greek regional unit centers");
            var centers = await _ypenService.GetCenters();
            //var centers = await _ypenService.GetYpenRegionCenters();
            return centers;
        }

        [HttpGet("RegionCenterData/{KALCODE}")]
        [ProducesResponseType(typeof(ApiResponse<RegionCenterDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<RegionCenterDto?> GetCenterUnitData([FromRoute] string KALCODE)
        {
            _logger.LogInformation($"Requesting center location for Greek regional Unit with id: {KALCODE}");
            var center = await _ypenService.GetCenter(KALCODE);
            //var units = await _ypenService.GetYpenRegionUnits();
            return center;
        }
        #endregion

        #region Init Endpoints
        //[Authorize]
        [Obsolete]
        [HttpGet("SaveRegionData")]
        [ProducesResponseType(typeof(ApiResponse<List<RegionUnit>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<List<RegionUnit>> GetRegionData()
        {
            _logger.LogInformation($"Requesting Greek regional unit shapes and centers from YPEN");
            var centers = await _ypenService.GetYpenRegionCenters();
            var units = await _ypenService.GetYpenRegionUnits();
            if (!centers.IsSuccess || !units.IsSuccess)
            {
                string error = centers.ErrorMessage ?? units.ErrorMessage ?? "Unknown Error on Units and Centers request";
                _logger.LogError($"Failed to retrieve region centers from YPEN: {error}");
                throw new Exception(error);
            }
            var persistResp = await _ypenService.PersistUnits(centers.Response!, units.Response!);
            return persistResp;
        }
        #endregion
    }
}

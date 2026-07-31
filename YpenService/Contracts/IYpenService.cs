using System;
using System.Collections.Generic;
using System.Text;
using YpenService.Models.Pollinator;
using YpenService.Models.Pollinator.Business;
using YpenService.Models.Pollinator.Persistence;
using YpenService.Models.Ypen;

namespace YpenService.Contracts
{
    public interface IYpenService
    {
        #region Units Services
        Task<List<RegionUnitDto>> GetUnits();
        Task<RegionUnitDto> GetUnit(string KALCODE);
        #endregion

        #region Centers Services
        Task<List<RegionCenterDto>> GetCenters();
        Task<RegionCenterDto> GetCenter(string KALCODE);
        #endregion

        #region Init Services
        Task<ServiceResponse<List<RegionUnitDto>>> GetYpenRegionUnits();
        Task<ServiceResponse<List<RegionCenterDto>>> GetYpenRegionCenters();
        Task<List<RegionUnit>> PersistUnits(List<RegionCenterDto> centers, List<RegionUnitDto> units);
        Task<List<RegionUnit>> ImportRegions();
        #endregion
    }
}

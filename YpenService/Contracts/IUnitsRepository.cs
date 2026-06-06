using System;
using System.Collections.Generic;
using System.Text;
using YpenService.Models.Pollinator.Persistence;

namespace YpenService.Contracts
{
    public interface IUnitsRepository
    {
        #region Units Repositories
        Task<List<RegionUnit>> GetUnitsAsync();
        Task<RegionUnit> GetUnitAsync(string KALCODE);
        #endregion

        #region Centers Repositories
        Task<List<RegionCenter>> GetCentersAsync();
        Task<RegionCenter> GetCenterAsync(string KALCODE);
        #endregion

        #region Init Repository
        Task<List<RegionUnit>> AddRegionsAsync(List<RegionUnit> units);
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using YpenService.Models.Pollinator;
using YpenService.Models.Pollinator.Persistence;
using YpenService.Models.Ypen;

namespace YpenService.Contracts
{
    public interface IYpenService
    {
        Task<ServiceResponse<List<RegionUnitsDto>>> GetRegionUnits();
        Task<ServiceResponse<List<RegionCentersDto>>> GetRegionCenters();
        Task<List<RegionUnits>> PersistUnits(List<RegionCentersDto> centers, List<RegionUnitsDto> units);
    }
}

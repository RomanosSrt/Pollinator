using System;
using System.Collections.Generic;
using System.Text;
using YpenService.Models.Pollinator.Persistence;

namespace YpenService.Contracts
{
    public interface IUnitsRepository
    {
        Task<List<RegionUnits>> Add(List<RegionUnits> units);
    }
}

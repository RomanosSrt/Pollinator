using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YpenService.Contracts;
using YpenService.Models.Pollinator.Persistence;

namespace YpenService.Helpers
{
    public class UnitsRepository(
        ILogger<UnitsRepository> logger,
        YpenDbContext dbCont, 
        IMapper mapper) : IUnitsRepository
    {
        public async Task<List<RegionUnits>> Add(List<RegionUnits> units)
        {
            logger.LogInformation("Adding the {UnitCount} Greek region units to DB", units.Count);
            foreach (var unit in units)
            {
                dbCont.RegionUnits.Add(unit);
            }
            await dbCont.SaveChangesAsync();
            return units;
        }
    }
}

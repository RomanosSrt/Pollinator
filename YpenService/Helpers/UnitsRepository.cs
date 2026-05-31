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
        public async Task<List<RegionUnitsDto>> Add(List<RegionUnitsDto> units)
        {
            logger.LogInformation("Adding the {UnitCount} Greek region units to DB", units.Count);
            foreach (var unit in units)
            {
                dbCont.RegionUnits.Add(mapper.Map<RegionUnits>(unit));
            }
            await dbCont.SaveChangesAsync();
            return units;
        }
    }
}

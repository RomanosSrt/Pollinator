using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YpenService.Contracts;
using YpenService.Models.Pollinator.Business;
using YpenService.Models.Pollinator.Persistence;

namespace YpenService.Helpers
{
    public class UnitsRepository(
        ILogger<UnitsRepository> logger,
        YpenDbContext dbCont, 
        IMapper mapper) : IUnitsRepository
    {
        #region Units Repositories
        public async Task<List<RegionUnit>> GetUnitsAsync()
        {
            logger.LogInformation("Acquiring all Greek region units from DB");
            List<RegionUnit> units = await dbCont.RegionUnits.ToListAsync();
            if (units == null || units.Count == 0)
            {
                logger.LogError("No Greek region units found in DB");
                throw new Exception("No Greek region units found in DB");
            }
            return units;
        }
        public async Task<RegionUnit> GetUnitAsync(string KALCODE)
        {
            logger.LogInformation($"Acquiring Greek region unit with id: {KALCODE} from DB");
            RegionUnit? unit = await dbCont.RegionUnits.FindAsync(KALCODE);
            if (string.IsNullOrEmpty(unit!.unit_KALCODE))
            {
                logger.LogError("No Greek region units found in DB");
                throw new Exception("No Greek region units found in DB");
            }
            return unit;
        }
        #endregion

        #region Centers Repositories
        public async Task<List<RegionCenter>> GetCentersAsync()
        {
            logger.LogInformation("Acquiring all Greek region units from DB");
            List<RegionUnit> units = await dbCont.RegionUnits.ToListAsync();
            if (units == null || units.Count == 0)
            {
                logger.LogError("No Greek region units found in DB");
                throw new Exception("No Greek region units found in DB");
            }
            List<RegionCenter> centers = mapper.Map<List<RegionCenter>>(units);
            return centers;
        }
        public async Task<RegionCenter> GetCenterAsync(string KALCODE)
        {
            logger.LogInformation($"Acquiring Greek region center with id: {KALCODE} from DB");
            RegionUnit? unit = await dbCont.RegionUnits.FindAsync(KALCODE);
            if (string.IsNullOrEmpty(unit!.unit_KALCODE))
            {
                logger.LogError($"No Greek region center found in DB with id: {KALCODE}");
                throw new Exception($"No Greek region center found in DB with id: {KALCODE}");
            }
            RegionCenter center = mapper.Map<RegionCenter>(unit);
            return center;
        }

        #endregion

        #region Init Repository
        public async Task<List<RegionUnit>> AddRegionsAsync(List<RegionUnit> units)
        {
            logger.LogInformation("Adding the {UnitCount} Greek region units to DB", units.Count);
            foreach (var unit in units)
            {
                dbCont.RegionUnits.Add(unit);
            }
            await dbCont.SaveChangesAsync();
            return units;
        }
        #endregion
    }
}

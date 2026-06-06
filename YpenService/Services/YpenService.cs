using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using YpenService.Contracts;
using YpenService.Models.Pollinator;
using YpenService.Models.Pollinator.Business;
using YpenService.Models.Pollinator.Persistence;
using YpenService.Models.Pollinator.Settings;
using YpenService.Models.Ypen;

namespace YpenService.Services
{
    public class YpenService(
        ILogger<YpenService> logger,
        IOptions<YpenSettings> settings,
        IOptions<DBSettings> dbSettings,
        IYpenClient client,
        IUnitsRepository repo,
        IMapper mapper,
        IMemoryCache cache) : IYpenService
    {
        #region Units Services
        public async Task<List<RegionUnitDto>> GetUnits()
        {
            string method = "GetUnits";
            logger.LogInformation($"IN Method {method} called");
            try
            {
                const string cacheKey = "region_units_cache";
                if (cache.TryGetValue(cacheKey, out List<RegionUnitDto>? cachedUnits))
                {
                    logger.LogInformation("Returning cached region units");
                    return [.. cachedUnits!];
                }
                List<RegionUnit> units = await repo.GetUnitsAsync();
                List<RegionUnitDto> mappedUnits = mapper.Map<List<RegionUnitDto>>(units);
                cache.Set(cacheKey, mappedUnits, GetCacheOptions());
                logger.LogInformation($"OUT Method {method}");
                return mappedUnits;
            }
            catch (Exception ex)
            {
                logger.LogError($"Method {method} failed with exception: {ex?.Message ?? "Unknown exception!"}");
                throw new Exception(ex?.Message ?? $"Exception on method {method} acquiring Region Centers.");
            }
        }
        public async Task<RegionUnitDto> GetUnit(string KALCODE)
        {
            string method = $"GetUnit/{KALCODE}";
            logger.LogInformation($"IN Method {method} called");
            try
            {
                RegionUnit unit = await repo.GetUnitAsync(KALCODE);
                logger.LogInformation($"OUT Method {method}");
                return mapper.Map<RegionUnitDto>(unit);
            }
            catch (Exception ex)
            {
                logger.LogError($"Method {method} failed with exception: {ex?.Message ?? "Unknown exception!"}");
                throw new Exception(ex?.Message ?? $"Exception on method {method} acquiring Region's {KALCODE}  shape.");
            }
        }
        #endregion

        #region Centers Services
        public async Task<List<RegionCenterDto>> GetCenters()
        {
            string method = "GetCenters";
            logger.LogInformation($"IN Method {method} called");
            try
            {
                const string cacheKey = "region_centers_cache";
                if (cache.TryGetValue(cacheKey, out List<RegionCenterDto>? cachedCenters))
                {
                    logger.LogInformation("Returning cached region centers");
                    return [.. cachedCenters!];
                }
                List<RegionCenter> centers = await repo.GetCentersAsync();
                cache.Set(cacheKey, centers, GetCacheOptions());
                logger.LogInformation($"OUT Method {method}");
                return mapper.Map<List<RegionCenterDto>>(centers);
            }
            catch (Exception ex)
            {
                logger.LogError($"Method {method} failed with exception: {ex?.Message ?? "Unknown exception!"}");
                throw new Exception(ex?.Message ?? $"Exception on method {method} acquiring Region Centers.");
            }
        }

        public async Task<RegionCenterDto> GetCenter(string KALCODE)
        {
            string method = $"GetCenter/{KALCODE}";
            logger.LogInformation($"IN Method {method} called");
            try
            {
                RegionCenter center = await repo.GetCenterAsync(KALCODE);
                logger.LogInformation($"OUT Method {method}");
                return mapper.Map<RegionCenterDto>(center);
            }
            catch (Exception ex)
            {
                logger.LogError($"Method {method} failed with exception: {ex?.Message ?? "Unknown exception!"}");
                throw new Exception(ex?.Message ?? $"Exception on method {method} acquiring Region {KALCODE} Center's  location.");
            }
        }

        #endregion


        #region Init Services
        public async Task<ServiceResponse<List<RegionUnitDto>>> GetYpenRegionUnits()
        {
            string method = "GetYpenRegionUnits";
            logger.LogInformation("IN Method {method} called", method);
            try
            {
                var resp = await client.GetAsync<RegionUnitsResponse>(settings.Value.RegionUnitsUrl);
                var units = mapper.Map<List<RegionUnitDto>>(resp);
                logger.LogInformation("OUT Method {method}", method);
                return new ServiceResponse<List<RegionUnitDto>>(units);
            }
            catch (Exception ex)
            {
                logger.LogError($"Method {method} failed with exception: {ex?.Message ?? "Unknown exception!"}");
                throw new Exception(ex?.Message ?? $"Exception on method {method} acquiring Region Units shapes");
            }
        }

        public async Task<ServiceResponse<List<RegionCenterDto>>> GetYpenRegionCenters()
        {
            string method = "GetYpenRegionCenters";
            logger.LogInformation("IN Method {method} called", method);
            try
            {
                var resp = await client.GetAsync<RegionCentersResponse>(settings.Value.RegionCentersUrl);
                var centers = mapper.Map<List<RegionCenterDto>>(resp);
                logger.LogInformation("OUT Method {method}", method);
                return new ServiceResponse<List<RegionCenterDto>>(centers);
            }
            catch (Exception ex)
            {
                logger.LogError($"Method {method} failed with exception: {ex?.Message ?? "Unknown exception!"}");
                throw new Exception(ex?.Message ?? $"Exception on method {method} acquiring Region Center locations");
            }
        }

        public async Task<List<RegionUnit>> PersistUnits(List<RegionCenterDto> centers, List<RegionUnitDto> units)
        {
            string method = "SaveRegionCenters&Units";
            logger.LogInformation("IN Method {method} called", method);
            try
            {
                var centersDict = centers.ToDictionary(u => u.KALCODE);
                var mergedUnits = units
                    .Select(unit =>
                    {
                        if (centersDict.TryGetValue(unit.KALCODE, out var center))
                        {
                            unit.Name = center.Name;
                            unit.Region = center.Region;
                            unit.Latitude = center.Latitude;
                            unit.Longitude = center.Longitude;
                        }
                        else
                        {
                            logger.LogWarning($"Method {method} could not find a matching Region Center for KALCODE {unit.KALCODE}");
                        }
                        return mapper.Map<RegionUnit>(unit);
                    })
                    .ToList();
                await repo.AddRegionsAsync(mergedUnits);
                logger.LogInformation($"Method {method} finished matching Region Centers to Units and stored to DB");
                return mergedUnits;
            }
            catch (Exception ex)
            {
                logger.LogError($"Method {method} failed to deserialize and store Units with exception: {ex?.Message ?? "Unknown exception!"}");
                throw new Exception(ex?.Message ?? $"Exception on method {method} acquiring and storing Region Center locations");
            }
        }
        #endregion

        private MemoryCacheEntryOptions GetCacheOptions()
        {
            return new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
        }
    }
}

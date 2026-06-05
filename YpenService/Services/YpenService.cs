using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using YpenService.Contracts;
using YpenService.Models.Pollinator;
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
        IUnitsRepository unitsRepo,
        IMapper mapper,
        IMemoryCache cache) : IYpenService
    {
        public async Task<ServiceResponse<List<RegionUnitsDto>>> GetRegionUnits()
        {
            string method = "GetRegionUnits";
            logger.LogInformation("IN Method {method} called", method);
            try
            {
                const string cacheKey = "region_units_cache";
                if (cache.TryGetValue(cacheKey, out List<RegionUnitsDto> cachedUnits))
                {
                    logger.LogInformation("Returning cached region units");
                    return new ServiceResponse<List<RegionUnitsDto>>(cachedUnits);
                }

                var resp = await client.GetAsync<RegionUnitsResponse>(settings.Value.RegionUnitsUrl);
                var units = mapper.Map<List<RegionUnitsDto>>(resp);
                //logger.LogInformation("OUT Method {method} got a response: {response}", method, JsonSerializer.Serialize(resp));


                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };
                cache.Set(cacheKey, units, cacheOptions);
                return new ServiceResponse<List<RegionUnitsDto>>(units);
            }
            catch (Exception ex)
            {
                logger.LogError($"Method {method} failed with exception: {ex?.Message ?? "Unknown exception!"}");
                throw new Exception(ex?.Message ?? $"Exception on method {method} acquiring Region Units shapes");
            }
        }

        public async Task<ServiceResponse<List<RegionCentersDto>>> GetRegionCenters()
        {
            string method = "GetRegionCenters";
            logger.LogInformation("IN Method {method} called", method);
            try
            {
                const string cacheKey = "region_centers_cache";
                if (cache.TryGetValue(cacheKey, out List<RegionCentersDto> cachedCenters))
                {
                    logger.LogInformation("Returning cached region centers");
                    return new ServiceResponse<List<RegionCentersDto>>(cachedCenters);
                }
                var resp = await client.GetAsync<RegionCentersResponse>(settings.Value.RegionCentersUrl);
                var centers = mapper.Map<List<RegionCentersDto>>(resp);
                //logger.LogInformation("OUT Method {method} got a response: {response}", method, JsonSerializer.Serialize(resp));

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                };
                cache.Set(cacheKey, centers, cacheOptions);
                return new ServiceResponse<List<RegionCentersDto>>(centers);
            }
            catch (Exception ex)
            {
                logger.LogError($"Method {method} failed with exception: {ex?.Message ?? "Unknown exception!"}");
                throw new Exception(ex?.Message ?? $"Exception on method {method} acquiring Region Center locations");
            }
        }

        public async Task<List<RegionUnits>> PersistUnits(List<RegionCentersDto> centers, List<RegionUnitsDto> units)
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
                        return mapper.Map<RegionUnits>(unit);
                    })
                    .ToList();
                logger.LogInformation($"Method {method} finished matching Region Centers to Units");
                await unitsRepo.Add(mergedUnits);
                //logger.LogInformation($"OUT Method {method} deserialized Units object: {mergedUnits}"); //TOO MUCH DATA TO LOG
                return mergedUnits;
            }
            catch (Exception ex)
            {
                logger.LogError($"Method {method} failed to deserialize and store Units with exception: {ex?.Message ?? "Unknown exception!"}");
                throw new Exception(ex?.Message ??  $"Exception on method {method} acquiring and storing Region Center locations");
            }
        }
    }
}

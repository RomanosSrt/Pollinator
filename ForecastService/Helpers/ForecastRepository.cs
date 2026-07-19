using AutoMapper;
using ForecastService.Contracts;
using ForecastService.Models.Pollinator.DAOs;
using ForecastService.Models.Pollinator.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using YpenService.Helpers;
using YpenService.Models.Pollinator.Business;
using YpenService.Models.Pollinator.Persistence;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ForecastService.Helpers
{
    public class ForecastRepository(
        ILogger<ForecastRepository> logger,
        ForecastDbContext dbCont,
        IMapper mapper) : IForecastRepository
    {
        public async Task<List<AirQualityDAO>> GetAirQualityAsync(List<RegionCenterDto> centers, DateOnly date)
        {
            string methodName = "GetAirQualityAsync";
            logger.LogInformation($"IN {methodName}");
            if (!centers.Any())
                throw new ArgumentException("Centers list cannot be empty");
            List<string> kalcodes = centers.Select(c => c.KALCODE).ToList();
            logger.LogInformation("Acquiring air quality data from DB");
            List<AirQualityDAO> airQualityData = await dbCont.AirQuality
                .Where(p => kalcodes.Contains(p.Kalcode) && p.Time == date)
                .ToListAsync();
            if (airQualityData == null || airQualityData.Count == 0)
            {
                logger.LogError("No Greek region units found in DB");
                throw new Exception("No Greek region units found in DB");
            }
            logger.LogInformation($"Air quality data acquired, total items: {airQualityData.Count}");
            logger.LogInformation($"OUT {methodName}");
            return airQualityData;
        }

        public async Task<List<WeatherDAO>> GetWeatherAsync(List<RegionCenterDto> centers, DateOnly date)
        {
            string methodName = "GetWeatherAsync";
            logger.LogInformation($"IN {methodName}");
            logger.LogInformation("Acquiring weather data from DB");
            if (!centers.Any())
                throw new ArgumentException("Centers list cannot be empty");
            List<string> kalcodes = centers.Select(c => c.KALCODE).ToList();
            List<WeatherDAO> weatherData = await dbCont.Weather
                .Where(p => kalcodes.Contains(p.Kalcode) && p.Time == date)
                .ToListAsync();
            if (weatherData == null || weatherData.Count == 0)
            {
                logger.LogError("No Greek region units found in DB");
                throw new Exception("No Greek region units found in DB");
            }
            logger.LogInformation($"Weather data acquired, total items: {weatherData.Count}");
            logger.LogInformation($"OUT {methodName}");
            return weatherData;
        }

        public async Task<List<AirQualityDAO>> UpdateAirQualityAsync(List<AirQualityDAO> airQualityData)
        {
            string methodName = "UpdateAirQualityAsync";
            logger.LogInformation($"IN {methodName}");
            logger.LogInformation("Updating air quality data on DB");
            await using var transaction = await dbCont.Database.BeginTransactionAsync();
            try
            {
                var delete = dbCont.AirQuality.ExecuteDeleteAsync();
                dbCont.AirQuality.AddRange(airQualityData);
                var save = dbCont.SaveChangesAsync();
                var commit = transaction.CommitAsync();
                await Task.WhenAll(delete, save, commit);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            logger.LogInformation($"Air quality data updated, total items: {airQualityData.Count}");
            logger.LogInformation($"OUT {methodName}");
            return airQualityData;
        }

        public async Task<List<WeatherDAO>> UpdateWeatherAsync(List<WeatherDAO> weatherData)
        {
            string methodName = "UpdateWeatherAsync";
            logger.LogInformation($"IN {methodName}");
            logger.LogInformation("Updating weather data on DB");
            await using var transaction = await dbCont.Database.BeginTransactionAsync();
            try
            {
                var delete = dbCont.AirQuality.ExecuteDeleteAsync();
                dbCont.Weather.AddRange(weatherData);
                var save = dbCont.SaveChangesAsync();
                var commit = transaction.CommitAsync();
                await Task.WhenAll(delete, save, commit);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            logger.LogInformation($"Weather data updated, total items: {weatherData.Count}");
            logger.LogInformation($"OUT {methodName}");
            return weatherData;
        }
    }
}

using ForecastService.Models.Pollinator.DAOs;
using YpenService.Models.Pollinator.Business;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Contracts
{
    public interface IForecastRepository
    {
        #region Init
        Task<List<WeatherDAO>> UpdateWeatherAsync(List<WeatherDAO> weatherData);
        Task<List<AirQualityDAO>> UpdateAirQualityAsync(List<AirQualityDAO> airQualityData);
        #endregion

        #region Weather
        Task<List<WeatherDAO>> GetWeatherAsync(List<RegionCenterDto> centers);
        #endregion

        #region AirQuality
        Task<List<AirQualityDAO>> GetAirQualityAsync(List<RegionCenterDto> centers);
        #endregion
    }
}

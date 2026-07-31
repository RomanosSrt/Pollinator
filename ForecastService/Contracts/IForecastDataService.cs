using ForecastService.Models.OpenMeteo.Responses;
using ForecastService.Models.Pollinator;
using ForecastService.Models.Pollinator.DTOs;
using ForecastService.Models.Pollinator.QueryParams;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Contracts
{
    public interface IForecastDataService
    {
        #region AirQuality Services
        Task<List<AirQualityDTO>> GetTotalAirQuality4D();
        Task<List<AirQualityDTO>> GetAirQuality4DById(string kalcode);
        #endregion

        #region Weather Services
        Task<List<WeatherDTO>> GetTotalWeather4D();
        Task<List<WeatherDTO>> GetWeather4DById(string kalcode);
        #endregion

        #region Init Services
        Task<ServiceResponse<List<AirQualityResponse>>> LoadAirQualForecast();
        Task<ServiceResponse<List<WeatherDTO>>> LoadWeatherForecast();
        Task<bool> CheckDBData();
        #endregion
    }
}

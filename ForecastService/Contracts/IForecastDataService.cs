using ForecastService.Models.OpenMeteo.Responses;
using ForecastService.Models.Pollinator;
using ForecastService.Models.Pollinator.QueryParams;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Contracts
{
    public interface IForecastDataService
    {
        Task<ServiceResponse<AirQualityResponse>> Get5DAirQualForecast(PollenIndexesParams queryParams);
        Task<ServiceResponse<WeatherForecastResponse>> Get5DWeatherForecast(WeatherParams queryParams);
    }
}

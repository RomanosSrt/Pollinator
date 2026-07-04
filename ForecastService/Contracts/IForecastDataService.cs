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
        #region Pollen
        //Task<T> Get
        #endregion

        #region External 
        Task<ServiceResponse<AirQualityResponse>> Get3DForecast(PollenIndexesParams queryParams);

        #endregion
    }
}

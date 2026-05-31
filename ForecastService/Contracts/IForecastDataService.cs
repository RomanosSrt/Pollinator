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
        Task<ServiceResponse<PollenIndexesResponse>> Get3DPollenIndexes(PollenIndexesParams queryParams);

        #endregion
    }
}

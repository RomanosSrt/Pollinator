using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Contracts
{
    public interface IForecastClient
    {
        Task<TResponse> GetAsync<TResponse>(string requestUri);
        Task<TResponse> PostAsync<TResponse>(string requestUri, object? content = null);
    }
}

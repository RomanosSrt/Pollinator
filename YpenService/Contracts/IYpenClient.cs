using System;
using System.Collections.Generic;
using System.Text;

namespace YpenService.Contracts
{
    public interface IYpenClient
    {
        Task<TResponse> GetAsync<TResponse>(string requestUri);
        Task<TResponse> PostAsync<TResponse>(string requestUri, object? content = null);
    }
}

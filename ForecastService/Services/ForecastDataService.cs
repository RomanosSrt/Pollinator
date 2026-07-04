using ForecastService.Contracts;
using ForecastService.Models.OpenMeteo.Responses;
using ForecastService.Models.Pollinator;
using ForecastService.Models.Pollinator.QueryParams;
using ForecastService.Models.Pollinator.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Events;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ForecastService.Services
{
    public class ForecastDataService(
        ILogger<ForecastDataService> _logger, 
        IOptions<OpenMeteoSettings> openMeteoSettings,
        IForecastClient client) : IForecastDataService
    {
        private readonly OpenMeteoSettings _openMeteoSettings = openMeteoSettings.Value;

        #region External
        public async Task<ServiceResponse<AirQualityResponse>> Get3DForecast(PollenIndexesParams queryParams)
        {
            string method = "Get3DForecast";
            _logger.LogInformation("IN Method {method} called with parameters: {parameters}", method, queryParams);
            try
            {
                var requestUri = CreateQueryUrl(queryParams, openMeteoSettings.Value.PollenBaseUrl);
                var resp = await client.GetAsync<AirQualityResponse>(requestUri);
                _logger.LogInformation("OUT Method {method} got a response: {response}", method, JsonSerializer.Serialize(resp));
                return new ServiceResponse<AirQualityResponse>(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR Method {method} failed with exception: {ex?.Message ?? "Unknown exception!"}");
                throw;
            }

        }

        private string CreateQueryUrl<TRequest>(TRequest queryParams, string url)
        {
            _logger.LogInformation($"Creating request with URL: {url} and query parameters: {queryParams}");
            var query = new QueryString();

            foreach (var property in typeof(TRequest).GetProperties())
            {
                var value = property.GetValue(queryParams);
                if (value is null)
                    continue;
                var jsonAttr = property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? property.Name;
                if (value is System.Collections.IEnumerable enumerable)
                {
                    var items = enumerable.Cast<object>().Select(i => i.ToString());
                    query = query.Add(jsonAttr, string.Join(",", items));
                    continue;
                }
                query = query.Add(jsonAttr, value.ToString());
            }

            if (string.IsNullOrEmpty(query.Value))
                throw new ArgumentException("No valid query parameters provided.");
            _logger.LogInformation($"Request url created successfully: {url} + {query.Value}");

            return url + query.Value.TrimStart('?');
        }
        #endregion
    }
}

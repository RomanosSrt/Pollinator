using ForecastService.Contracts;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ForecastService.Models.OpenMeteo;
using ForecastService.Models.OpenMeteo.Responses;

namespace ForecastService
{
    public class ForecastClient : IForecastClient
    {
        private readonly ILogger<ForecastClient> _logger;
        private readonly HttpClient _httpClient;

        public ForecastClient(ILogger<ForecastClient> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public Task<TResponse> GetAsync<TResponse>(string requestUri) => SendAsync<TResponse>(new HttpRequestMessage(HttpMethod.Get, requestUri));
        public Task<TResponse> PostAsync<TResponse>(string requestUri, object? content = null)
        {
            var postRequest = new HttpRequestMessage(HttpMethod.Post, requestUri);
            if (content != null)
                postRequest.Content = JsonContent.Create(content);

            return SendAsync<TResponse>(postRequest);
        }

        private async Task<TResponse> SendAsync<TResponse>(HttpRequestMessage request)
        {
            _logger.LogInformation($"Sending Pollinator request to {request.RequestUri}");
            try
            {
                _logger.LogInformation("Pollinator Request: " + request);
                using var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Open-Meteo Response status code not successful: {(int)response.StatusCode} ({response.ReasonPhrase}).");

                if (response.StatusCode == HttpStatusCode.NoContent)
                    throw new HttpRequestException($"Pollinator Request not successful: {(int)response.StatusCode} ({response.ReasonPhrase}).");

                var contentString = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Open-Meteo Response: " + response);
                return Deserialize<TResponse>(contentString);
            }
            catch (TaskCanceledException ex)
            {
                throw new TimeoutException(ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex.InnerException);
            }
        }

        private static TResponse Deserialize<TResponse>(string content)
        {
            try
            {
                if (content.TrimStart().StartsWith("{") &&
                    typeof(IOpenMeteoResponse).IsAssignableFrom(typeof(TResponse).GetGenericArguments()[0]))
                    content = $"[{content}]";
                var resp = JsonSerializer.Deserialize<TResponse>(content);
                return resp == null ? throw new JsonException("Pollinator failed to deserialize the response content") : resp;
            }
            catch (JsonException ex)
            {
                var error = JsonSerializer.Deserialize<ErrorStatus>(content);
                if (error != null)
                {
                    throw new Exception($"Pollinator returned error with code ({error.Error}): {error.ErrorDescription}");
                }
                throw new JsonException("Pollinator failed to deserialize the response content with error: " + ex.Message);
            }
        }
    }
}

using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using YpenService.Contracts;
using YpenService.Models.Ypen;

namespace YpenService
{
    public class YpenClient : IYpenClient
    {
        private readonly ILogger<YpenClient> _logger;
        private readonly HttpClient _httpClient;

        public YpenClient(ILogger<YpenClient> logger, HttpClient httpClient)
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
                    throw new HttpRequestException($"YPEN Response status code not successful: {(int)response.StatusCode} ({response.ReasonPhrase}).");

                if (response.StatusCode == HttpStatusCode.NoContent)
                    throw new HttpRequestException($"Pollinator Request not successful: {(int)response.StatusCode} ({response.ReasonPhrase}).");

                var contentString = await response.Content.ReadAsStringAsync();

                //_logger.LogInformation("YPEN Response: " + response);
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
                var resp = JsonSerializer.Deserialize<TResponse>(content);
                return resp == null ? throw new JsonException("Pollinator failed to deserialize the response content") : resp;
            }
            catch (JsonException ex)
            {
                var error = JsonSerializer.Deserialize<ErrorStatus>(content);
                if (error != null && !string.IsNullOrWhiteSpace(error.Error.Text))
                {
                    throw new Exception($"Pollinator returned error with code ({error.Error.ExceptionCode}): {error.Error.Text}");
                }
                throw new JsonException("Pollinator failed to deserialize the response with error: " + ex.Message);
            }
        }
    }
}

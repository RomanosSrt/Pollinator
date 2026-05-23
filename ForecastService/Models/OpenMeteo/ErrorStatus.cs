using System.Text.Json.Serialization;

namespace ForecastService.Models.OpenMeteo
{
    public class ErrorStatus
    {
        [JsonPropertyName("error")]
        public bool Error { get; set; }
        [JsonPropertyName("reason")]
        public string ErrorDescription { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace ForecastService.Models.Pollinator.QueryParams
{
    public class WeatherParams
    {
        [Required]
        [JsonPropertyName("latitude")]
        public List<float> Latitudes { get; set; } = new List<float>();
        [Required]
        [JsonPropertyName("longitude")]
        public List<float> Longtitudes { get; set; } = new List<float>();
        [JsonPropertyName("daily")]
        public List<WeatherIndicator> DailyWeatherIndicators { get; set; } = new List<WeatherIndicator>();
        //[JsonPropertyName("forecast_days")]
        //public int ForecastPeriod { get; set; } = 5;
    }
}

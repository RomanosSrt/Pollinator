using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace ForecastService.Models.Pollinator.QueryParams
{
    public class WeatherParams : OpenMeteoParams
    {
        [JsonPropertyName("daily")]
        public List<WeatherIndicator> DailyWeatherIndicators { get; set; } = new List<WeatherIndicator>();
        //[JsonPropertyName("forecast_days")]
        //public int ForecastPeriod { get; set; } = 5;
    }
}

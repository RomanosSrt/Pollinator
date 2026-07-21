using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace ForecastService.Models.Pollinator.QueryParams
{
    public class AirQualityParams : OpenMeteoParams
    {
        [JsonPropertyName("hourly")]
        public List<AirQualityIndicator> HourlyAirParams { get; set; } = new List<AirQualityIndicator>();
        //[JsonPropertyName("current")]
        //public List<PollenType> CurrentPollenTypes { get; set; } = new List<PollenType>();
        //[JsonPropertyName("forecast_days")]
        //public int ForecastPeriod { get; set; } = 5;
    }
}

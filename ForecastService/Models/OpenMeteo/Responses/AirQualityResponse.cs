using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Models.OpenMeteo.Responses
{
    public class AirQualityResponse : IOpenMeteoResponse
    {
        public double latitude { get; set; } = new();
        public double longitude { get; set; } = new();
        public double generationtime_ms { get; set; } = new();
        public int utc_offset_seconds { get; set; } = new();
        public string timezone { get; set; } = string.Empty;
        public string timezone_abbreviation { get; set; } = string.Empty;
        public double elevation { get; set; } = new();
        public int? location_id { get; set; } = new();
        public HourlyUnits hourly_units { get; set; } = new();
        public Hourly hourly { get; set; } = new();
    }
}

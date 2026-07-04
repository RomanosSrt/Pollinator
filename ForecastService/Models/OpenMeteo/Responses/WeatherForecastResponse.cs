using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Models.OpenMeteo.Responses
{
    public class WeatherForecastResponse
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public string timezone { get; set; } = string.Empty;
        public string timezone_abbreviation { get; set; } = string.Empty;
        public int elevation { get; set; }
        public HourlyUnits hourly_units { get; set; } = new();
        public Hourly hourly { get; set; } = new();
        public int? location_id { get; set; }
    }
}

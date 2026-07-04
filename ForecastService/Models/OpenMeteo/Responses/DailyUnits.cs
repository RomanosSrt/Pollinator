using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Models.OpenMeteo.Responses
{
    public class DailyUnits
    {
        public string time {  get; set; } = string.Empty;
        public string weather_code { get; set; } = string.Empty;
        public string temperature_2m_max { get; set; } = string.Empty;
        public string temperature_2m_min { get; set; } = string.Empty;
        public string wind_speed_10m_max { get; set; } = string.Empty;
        public string precipitation_probability_max { get; set; } = string.Empty;
        public string relative_humidity_2m_max { get; set; } = string.Empty;
    }
}

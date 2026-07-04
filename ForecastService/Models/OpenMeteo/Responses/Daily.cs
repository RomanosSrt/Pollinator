using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Models.OpenMeteo.Responses
{
    public class Daily
    {
        public List<string> time { get; set; } = new();
        public List<int> weather_code { get; set; } = new();
        public List<double> temperature_2m_max { get; set; } = new();
        public List<double> temperature_2m_min { get; set; } = new();
        public List<double> wind_speed_10m_max { get; set; } = new();
        public List<int> precipitation_probability_max { get; set; } = new();
        public List<int> relative_humidity_2m_max { get; set; } = new();
    }
}

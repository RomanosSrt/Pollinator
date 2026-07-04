using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Models.OpenMeteo.Responses
{
    public class HourlyUnits
    {
        public string time { get; set; } = string.Empty;
        public string dust { get; set; } = string.Empty;
        public string alder_pollen { get; set; } = string.Empty;
        public string birch_pollen { get; set; } = string.Empty;
        public string grass_pollen { get; set; } = string.Empty;
        public string mugwort_pollen { get; set; } = string.Empty;
        public string olive_pollen { get; set; } = string.Empty;
        public string ragweed_pollen { get; set; } = string.Empty;
        public string pm10 { get; set; } = string.Empty;
        public string pm2_5 { get; set; } = string.Empty;
        public string european_aqi { get; set; } = string.Empty;
        public string ozone { get; set; } = string.Empty;
        public string nitrogen_dioxide { get; set; } = string.Empty;
    }
}

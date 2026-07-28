using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Models.OpenMeteo.Responses
{
    public class Hourly
    {
        public List<string> time { get; set; } = new();
        public List<float?>? dust { get; set; } = new();
        public List<float?>? alder_pollen { get; set; } = new();
        public List<float?>? birch_pollen { get; set; } = new();
        public List<float?>? grass_pollen { get; set; } = new();
        public List<float?>? mugwort_pollen { get; set; } = new();
        public List<float?>? olive_pollen { get; set; } = new();
        public List<float?>? ragweed_pollen { get; set; } = new();
        public List<float?>? pm10 { get; set; } = new();
        public List<float?>? pm2_5 { get; set; } = new();
        public List<float?>? european_aqi { get; set; } = new();
        public List<float?>? ozone { get; set; } = new();
        public List<float?>? nitrogen_dioxide { get; set; } = new();
    }
}

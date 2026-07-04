using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Models.OpenMeteo.Responses
{
    public class Hourly
    {
        public List<string> time { get; set; } = new();
        public List<double?>? dust { get; set; } = new();
        public List<double?>? alder_pollen { get; set; } = new();
        public List<double?>? birch_pollen { get; set; } = new();
        public List<double?>? grass_pollen { get; set; } = new();
        public List<double?>? mugwort_pollen { get; set; } = new();
        public List<double?>? olive_pollen { get; set; } = new();
        public List<double?>? ragweed_pollen { get; set; } = new();
        public List<double?>? pm10 { get; set; } = new();
        public List<double?>? pm2_5 { get; set; } = new();
        public List<int?>? european_aqi { get; set; } = new();
        public List<double?>? ozone { get; set; } = new();
        public List<double?>? nitrogen_dioxide { get; set; } = new();
        public List<double?>? temperature_2m { get; set; } = new();
        public List<double?>? wind_speed_10m { get; set; } = new();
        public List<int?>? weather_code { get; set; } = new();
        public List<int?>? precipitation_probability { get; set; } = new();
        public List<int?>? relative_humidity_2m { get; set; } = new();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace ForecastService.Models.Pollinator.QueryParams
{
    public class PollenIndexesParams
    {
        [Required]
        [JsonPropertyName("latitude")]
        public List<float> Latitudes { get; set; } = new List<float>();
        [Required]
        [JsonPropertyName("longitude")]
        public List<float> Longtitudes { get; set; } = new List<float>();
        [JsonPropertyName("hourly")]
        public List<PollenType> HourlyPollenTypes { get; set; } = new List<PollenType>();
        [JsonPropertyName("current")]
        public List<PollenType> CurrentPollenTypes { get; set; } = new List<PollenType>();
        [Required]
        [JsonPropertyName("forecast_days")]
        public int ForecastPeriod { get; set; } = 3;
        
        //[Required]
        //[JsonPropertyName("timezone")]
        //public string Timezone { get; set; } = "auto";
        //[Required]
        //[JsonPropertyName("domains")]
        //public string Domains { get; set; } = "cams_europe";

    }
}

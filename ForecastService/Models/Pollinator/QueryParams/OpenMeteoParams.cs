using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace ForecastService.Models.Pollinator.QueryParams
{
    public class OpenMeteoParams
    {
        [Required]
        [JsonPropertyName("latitude")]
        public List<double> Latitudes { get; set; } = new List<double>();
        [Required]
        [JsonPropertyName("longitude")]
        public List<double> Longitudes { get; set; } = new List<double>();
    }
}

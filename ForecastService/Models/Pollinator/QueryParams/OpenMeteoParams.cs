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
        public List<float> Latitudes { get; set; } = new();
        [Required]
        [JsonPropertyName("longitude")]
        public List<float> Longitudes { get; set; } = new();
    }
}

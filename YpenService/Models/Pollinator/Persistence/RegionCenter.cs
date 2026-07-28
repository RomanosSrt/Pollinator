using System;
using System.Collections.Generic;
using System.Text;

namespace YpenService.Models.Pollinator.Persistence
{
    public class RegionCenter
    {
        public string KALCODE { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public float Latitude { get; set; } = 0f;
        public float Longitude { get; set; } = 0f;
    }
}

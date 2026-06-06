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
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}

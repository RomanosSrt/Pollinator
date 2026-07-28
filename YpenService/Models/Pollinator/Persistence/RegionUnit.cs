using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace YpenService.Models.Pollinator.Persistence
{
    public class RegionUnit
    {
        public string unit_KALCODE { get; set; } = string.Empty;
        public string unit_Center { get; set; } = string.Empty;
        public string unit_Name { get; set; } = string.Empty;
        //public string unit_NameCheck { get; set; } = string.Empty;
        public float unit_Latitude { get; set; }  = 0f;
        public float unit_Longitude { get; set; } = 0f;
        public required Geometry unit_Shapes { get; set; }
        public float unit_Area { get; set; } = 0f;
    }
}

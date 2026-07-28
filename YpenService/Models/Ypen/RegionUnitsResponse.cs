using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace YpenService.Models.Ypen
{
    public class RegionUnitsResponse
    {
        public string type { get; set; } = string.Empty;
        public int totalFeatures { get; set; }
        public List<UnitFeature> features { get; set; } = new List<UnitFeature>();
        public Crs crs { get; set; } = new Crs();
    }

    public class UnitFeature
    {
        public string type { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        [JsonPropertyName("geometry")]
        public UnitsGeometry geometry { get; set; } = new UnitsGeometry();
        public string geometry_name { get; set; } = string.Empty;
        public UnitProperties properties { get; set; } = new UnitProperties();
    }

    public class UnitsGeometry
    {
        public string type { get; set; } = string.Empty;
        public List<List<List<List<float>>>> coordinates { get; set; } = new();
    }

    public class UnitProperties
    {
        public string KALCODE { get; set; } = string.Empty;
        public string LEKTIKO { get; set; } = string.Empty;
        public float AREA_km2 { get; set; }
        public int MON_2011 { get; set; }
        public int MON_2001 { get; set; }
        public int MON_1991 { get; set; }
        public int PRAGM_2011 { get; set; }
        public int PRAGM_2001 { get; set; }
        public int PRAGM_1991 { get; set; }
    }
}

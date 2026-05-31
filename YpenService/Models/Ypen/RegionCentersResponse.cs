using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace YpenService.Models.Ypen
{
    public class RegionCentersResponse
    {
        public string type { get; set; } = string.Empty;
        public int totalFeatures { get; set; }
        //[JsonPropertyName("features")]
        public List<CenterFeature> features { get; set; } = new List<CenterFeature>();
        public Crs crs { get; set; } = new Crs();
    }

    public class CenterFeature
    {
        public string type { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        //[JsonPropertyName("geometry")]
        public CenterGeometry geometry { get; set; } = new CenterGeometry();
        public string geometry_name { get; set; } = string.Empty;
        //[JsonPropertyName("properties")]
        public Properties properties { get; set; } = new Properties();
    }

    public class CenterGeometry
    {
        public string type { get; set; } = string.Empty;
        public List<double> coordinates { get; set; } = new List<double>();
    }

    public class Properties
    {
        public string KALCODE { get; set; } = string.Empty;
        public double LAT { get; set; }
        public double LON { get; set; }
        public string EDRA { get; set; } = string.Empty;    
        public string PE_ENOTHTA { get; set; } = string.Empty;
        public int MON_2011 { get; set; } 
        public int DFACT_2011 { get; set; }
        public int MON_2001 { get; set; }
        public int DFACT_2001 { get; set; }
        public int MON_1991 { get; set; }
        public int DFACT_1991 { get; set; }
    }

}

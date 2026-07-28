using System.Text.Json.Serialization;

namespace API.Application.DTOs.PlotHandling
{
    public class EsriJsonRoot
    {
        [JsonPropertyName("features")]
        public List<EsriFeature> features { get; set; } = new();
    }

    public class EsriFeature
    {
        [JsonPropertyName("attributes")]
        public EsriAttributes attributes { get; set; } = new();
        [JsonPropertyName("geometry")]
        public EsriGeometry geometry { get; set; } = new();
    }

    public class EsriAttributes
    {
        [JsonPropertyName("KAEK")]
        public string KAEK { get; set; } = string.Empty;
        [JsonPropertyName("AREA")]
        public float Area { get; set; }
    }

    public class EsriGeometry
    { 
        [JsonPropertyName("rings")]
        public float[][][] rings { get; set; } = [];
    }
}

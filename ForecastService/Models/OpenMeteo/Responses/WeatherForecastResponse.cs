namespace ForecastService.Models.OpenMeteo.Responses
{
    public class WeatherForecastResponse : IOpenMeteoResponse
    {
        public float latitude { get; set; } = 0;
        public float longitude { get; set; } = 0;
        public float generationtime_ms { get; set; } = 0;
        public int utc_offset_seconds { get; set; }
        public string timezone { get; set; } = string.Empty;    
        public string timezone_abbreviation { get; set; } = string.Empty;
        public float elevation { get; set; } = 0;
        public DailyUnits daily_units { get; set; } = new();
        public Daily daily { get; set; } = new();
        public int? location_id { get; set; }
    }
}

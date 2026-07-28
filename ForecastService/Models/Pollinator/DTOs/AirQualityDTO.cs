namespace ForecastService.Models.Pollinator.DTOs
{
    public class AirQualityDTO
    {

        //public Guid Id { get; set; } = new Guid();
        public string Kalcode { get; set; } = string.Empty;
        public float Dust { get; set; } = 0;
        public float AlderPollen { get; set; } = 0;
        public float BirchPollen { get; set; } = 0;
        public float GrassPollen { get; set; } = 0;
        public float MugwortPollen { get; set; } = 0;
        public float OlivePollen { get; set; } = 0;
        public float RagweedPollen { get; set; } = 0;
        public float PM10 { get; set; } = 0;
        public float PM2_5 { get; set; } = 0;
        public float AQI { get; set; } = 0;
        public float O3 { get; set; } = 0;
        public float NO2 { get; set; } = 0;
        public DateOnly Time { get; set; } = DateOnly.MinValue;
    }
}
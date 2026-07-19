namespace ForecastService.Models.Pollinator.DAOs
{
    public class AirQualityDAO
    {
        //public Guid Id { get; set; } = new Guid();
        public string Kalcode { get; set; } = string.Empty;
        public double Dust { get; set; } = 0;
        public double AlderPollen { get; set; } = 0.0;
        public double BirchPollen { get; set; } = 0.0;
        public double GrassPollen { get; set; } = 0.0; 
        public double MugwortPollen { get; set; } = 0.0;
        public double OlivePollen { get; set; } = 0.0;
        public double RagweedPollen { get; set; } = 0.0;
        public double PM10 { get; set; } = 0.0;
        public double PM2_5 { get; set; } = 0.0;
        public double AQI { get; set; } = 0.0;
        public double O3 { get; set; } = 0.0;
        public double NO2 { get; set; } = 0.0;
        public DateOnly Time { get; set; } = DateOnly.MinValue;
    }
}
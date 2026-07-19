namespace ForecastService.Models.Pollinator.DTOs
{
    public class WeatherDTO
    {
        //public Guid Id { get; set; } = new Guid();
        public string Kalcode { get; set; } = string.Empty;
        public int WmoCode { get; set; }
        public double TemperatureMax { get; set; }
        public double TemperatureMin { get; set; }
        public double WindSpeed { get; set; }
        public double PercipitationPct { get; set; }
        public double Humidity { get; set; }
        public DateOnly Time { get; set; } = DateOnly.MinValue;
    }
}
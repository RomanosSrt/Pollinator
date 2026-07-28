namespace ForecastService.Models.Pollinator.DTOs
{
    public class WeatherDTO
    {
        //public Guid Id { get; set; } = new Guid();
        public string Kalcode { get; set; } = string.Empty;
        public int WmoCode { get; set; } = 0;
        public float TemperatureMax { get; set; } = 0;
        public float TemperatureMin { get; set; } = 0;
        public float WindSpeed { get; set; } = 0;
        public float PrecipitationPct { get; set; } = 0;
        public float Humidity { get; set; } = 0;
        public DateOnly Time { get; set; } = DateOnly.MinValue;
    }
}
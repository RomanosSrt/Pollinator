namespace ForecastService.Models.Pollinator.DAOs;

public class WeatherDAO
{
    //public Guid Id { get; set; } = new Guid();
    public string Kalcode { get; set; } = string.Empty;
    public int WmoCode { get; set; }
    public double TemperatureMax { get; set; }
    public double TemperatureMin { get; set; }
    public double WindSpeed { get; set; }
    public double PrecipitationPct { get; set; }
    public double Humidity { get; set; }
    public DateOnly Time { get; set; } = DateOnly.MinValue;
}
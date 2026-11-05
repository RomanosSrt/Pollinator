using ForecastService.Contracts;

namespace Server.Application.Services.System
{
    public class ForecastRefreshService(IServiceScopeFactory scopes, ILogger<ForecastRefreshService> log) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromDays(1));
            do
            {
                try
                {
                    using var scope = scopes.CreateScope();
                    var forecast = scope.ServiceProvider.GetRequiredService<IForecastDataService>();
                    if (await forecast.CheckDBData())
                    {
                        await forecast.LoadAirQualForecast();
                        await forecast.LoadWeatherForecast();
                        log.LogInformation("Forecast data refreshed");
                    }
                }
                catch (Exception ex) { log.LogError(ex, "Forecast refresh failed"); }
            } while (await timer.WaitForNextTickAsync(ct));
        }
    }
}

using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.TelemetryConverters;

namespace API.Application.Services.System
{
    public static class LogService
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            bool isDev = builder.Environment.IsDevelopment();

            if (!isDev)
                builder.Services.AddApplicationInsightsTelemetry();

            builder.Host.UseSerilog((context, services, logger) =>
            {
                logger
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Application", "PollinatorAPI")
                    .WriteTo.Console();

                if (isDev)
                    logger.WriteTo.File(
                        "Logs/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext} - {Message:lj}{NewLine}{Exception}");
                else
                    logger.WriteTo.ApplicationInsights(
                        services.GetRequiredService<TelemetryConfiguration>(),
                        TelemetryConverter.Traces);
            });
        }
    }
}

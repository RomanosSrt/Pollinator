using API.Application.Services.System;
using API.Infrastructure.Persistence;
using API.Middleware;
using ForecastService.Contracts;
using ForecastService.Services;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.IO.Converters;
using Serilog;
using YpenService.Contracts;
using YpenService.Helpers;
using YpenService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.ConfigureSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Failed to build, connectionstring empty.");

builder.Services.AddPersistenceServices(connectionString);
builder.Services.RegisterPollinatorServices(builder.Configuration);
builder.Services.AddForecastServices(builder.Configuration);
builder.Services.AddYpenServices(builder.Configuration);

var app = builder.Build();
app.Logger.LogInformation("Application started at {Date}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")); 
using (var scope = app.Services.CreateScope())
{
    try { 
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
        var forecastDbContext = scope.ServiceProvider.GetRequiredService<ForecastDbContext>();
        forecastDbContext.Database.Migrate();
        var ypenDbContext = scope.ServiceProvider.GetRequiredService<YpenDbContext>();
        ypenDbContext.Database.Migrate();
        var forecastServices = scope.ServiceProvider.GetRequiredService<IForecastDataService>();
        var ypenServices = scope.ServiceProvider.GetRequiredService<IYpenService>();
        if (await forecastServices.CheckDBData())
        {
            await ypenServices.ImportRegions();
            await forecastServices.LoadAirQualForecast();
            await forecastServices.LoadWeatherForecast();
        }
    } catch (Exception ex)
    {
        app.Logger.LogError("Database Migration failed with error: {ErrorMessage}", ex.Message);
    }
}
app.Logger.LogInformation("Database migration completed successfully");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCompression();
app.UseHttpsRedirection();
//app.UseCors("AllowFrontend"); // Must come BEFORE UseAuthentication/UseAuthorization
app.UseAuthentication();        //Must come BEFORE UseAuthorization
app.UseAuthorization();
app.UseMiddleware<ResponseMiddleware>();

app.MapControllers();

app.Run();

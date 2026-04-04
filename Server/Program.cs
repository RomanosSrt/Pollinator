using API.Application.Services.System;
using API.Infrastructure.Persistence;
using API.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.ConfigureSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Failed to build, connectionstring empty.");
if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception();
}

builder.Services.AddPersistenceServices(connectionString);
builder.Services.RegisterServices();

var app = builder.Build();
app.Logger.LogInformation("Application started at {Date}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")); 
using (var scope = app.Services.CreateScope())
{
    try { 
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    } catch (Exception ex)
    {
        app.Logger.LogInformation("Database Migration failed with error {exception}", ex.Message);
        throw;
    }
}
app.Logger.LogInformation("Database migration completed successfully");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ResponseMiddleware>();

app.MapControllers();

app.Run();

using ForecastService.Models.Pollinator.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Helpers
{
    public class WeatherConfiguration : IEntityTypeConfiguration<WeatherDAO>
    {
        public void Configure(EntityTypeBuilder<WeatherDAO> builder)
        {
            builder.ToTable("Weather");
            builder.HasKey(w => new { w.Kalcode, w.Time });
            builder.Property(prop => prop.Time)
                .IsRequired()
                .HasColumnName("Time");
            
            builder.Property<int?>(prop => prop.WmoCode)
                //.HasColumnType("")
                .HasColumnName("WmoCode");
            builder.Property<double?>(prop => prop.TemperatureMax)
                //.HasColumnType("")
                .HasColumnName("TemperatureMax");
            builder.Property<double?>(prop => prop.TemperatureMin)
                //.HasColumnType("")
                .HasColumnName("TemperatureMin");
            builder.Property<double?>(prop => prop.WindSpeed)
                //.HasColumnType("")
                .HasColumnName("WindSpeed");
            builder.Property<double?>(prop => prop.PrecipitationPct)
                //.HasColumnType("")
                .HasColumnName("Percipitation");
            builder.Property<double?>(prop => prop.Humidity)
                //.HasColumnType("")
                .HasColumnName("Humidity");
        }
    }
}

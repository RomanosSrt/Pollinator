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
            builder.ToTable("AirQuality");
            builder.HasKey(w => w.Kalcode);
            builder.Property(prop => prop.Time)
                .IsRequired()
                .HasColumnName("Time");
            builder.HasIndex(i => new { i.Time, i.Kalcode });

            builder.Property<int?>(prop => prop.WmoCode)
                //.HasColumnType("")
                .HasColumnName("Dust");
            builder.Property<double?>(prop => prop.TemperatureMax)
                //.HasColumnType("")
                .HasColumnName("Dust");
            builder.Property<double?>(prop => prop.TemperatureMin)
                //.HasColumnType("")
                .HasColumnName("Dust");
            builder.Property<double?>(prop => prop.WindSpeed)
                //.HasColumnType("")
                .HasColumnName("Dust");
            builder.Property<double?>(prop => prop.PercipitationPct)
                //.HasColumnType("")
                .HasColumnName("Dust");
            builder.Property<double?>(prop => prop.Humidity)
                //.HasColumnType("")
                .HasColumnName("Dust");
        }
    }
}

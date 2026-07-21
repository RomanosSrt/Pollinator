using ForecastService.Models.Pollinator.DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace ForecastService.Helpers
{
    public class AirQualityConfiguration : IEntityTypeConfiguration<AirQualityDAO>
    {
        public void Configure(EntityTypeBuilder<AirQualityDAO> builder)
        {
            builder.ToTable("AirQuality");
            builder.HasKey(q => new { q.Time, q.Kalcode });
            builder.Property(prop => prop.Time)
                .IsRequired()
                .HasColumnName("Time");

            builder.Property<double?>(prop => prop.Dust)
                //.HasColumnType("")
                .HasColumnName("Dust");
            builder.Property<double?>(prop => prop.AlderPollen)
                //.HasColumnType("")
                .HasColumnName("AlderPollen");
            builder.Property<double?>(prop => prop.BirchPollen)
                //.HasColumnType("")
                .HasColumnName("BirchPollen");
            builder.Property<double?>(prop => prop.GrassPollen)
                //.HasColumnType("")
                .HasColumnName("GrassPollen");
            builder.Property<double?>(prop => prop.MugwortPollen)
                //.HasColumnType("")
                .HasColumnName("MugwortPollen");
            builder.Property<double?>(prop => prop.OlivePollen)
                //.HasColumnType("")
                .HasColumnName("OlivePollen");
            builder.Property<double?>(prop => prop.RagweedPollen)
                //.HasColumnType("")
                .HasColumnName("RagweedPollen");
            builder.Property<double?>(prop => prop.PM10)
                //.HasColumnType("")
                .HasColumnName("PM10");
            builder.Property<double?>(prop => prop.PM2_5)
                //.HasColumnType("")
                .HasColumnName("PM2_5");
            builder.Property<double?>(prop => prop.AQI)
                //.HasColumnType("")
                .HasColumnName("AQI");
            builder.Property<double?>(prop => prop.O3)
                //.HasColumnType("")
                .HasColumnName("O3");
            builder.Property<double?>(prop => prop.NO2)
                //.HasColumnType("")
                .HasColumnName("NO2");
        }
    }
}

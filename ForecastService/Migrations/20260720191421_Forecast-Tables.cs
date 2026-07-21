using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastService.Migrations
{
    /// <inheritdoc />
    public partial class ForecastTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "AirQuality",
                columns: table => new
                {
                    Kalcode = table.Column<string>(type: "text", nullable: false),
                    Dust = table.Column<double>(type: "double precision", nullable: false),
                    AlderPollen = table.Column<double>(type: "double precision", nullable: false),
                    BirchPollen = table.Column<double>(type: "double precision", nullable: false),
                    GrassPollen = table.Column<double>(type: "double precision", nullable: false),
                    MugwortPollen = table.Column<double>(type: "double precision", nullable: false),
                    OlivePollen = table.Column<double>(type: "double precision", nullable: false),
                    RagweedPollen = table.Column<double>(type: "double precision", nullable: false),
                    PM10 = table.Column<double>(type: "double precision", nullable: false),
                    PM2_5 = table.Column<double>(type: "double precision", nullable: false),
                    AQI = table.Column<double>(type: "double precision", nullable: false),
                    O3 = table.Column<double>(type: "double precision", nullable: false),
                    NO2 = table.Column<double>(type: "double precision", nullable: false),
                    Time = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirQuality", x => x.Kalcode);
                });

            migrationBuilder.CreateTable(
                name: "Weather",
                columns: table => new
                {
                    Kalcode = table.Column<string>(type: "text", nullable: false),
                    WmoCode = table.Column<int>(type: "integer", nullable: false),
                    TemperatureMax = table.Column<double>(type: "double precision", nullable: false),
                    TemperatureMin = table.Column<double>(type: "double precision", nullable: false),
                    WindSpeed = table.Column<double>(type: "double precision", nullable: false),
                    Percipitation = table.Column<double>(type: "double precision", nullable: false),
                    Humidity = table.Column<double>(type: "double precision", nullable: false),
                    Time = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weather", x => x.Kalcode);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirQuality_Time_Kalcode",
                table: "AirQuality",
                columns: new[] { "Time", "Kalcode" });

            migrationBuilder.CreateIndex(
                name: "IX_Weather_Time_Kalcode",
                table: "Weather",
                columns: new[] { "Time", "Kalcode" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirQuality");

            migrationBuilder.DropTable(
                name: "Weather");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForecastService.Migrations
{
    /// <inheritdoc />
    public partial class WeatherAndAirQualKeyFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Weather",
                table: "Weather");

            migrationBuilder.DropIndex(
                name: "IX_Weather_Time_Kalcode",
                table: "Weather");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AirQuality",
                table: "AirQuality");

            migrationBuilder.DropIndex(
                name: "IX_AirQuality_Time_Kalcode",
                table: "AirQuality");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weather",
                table: "Weather",
                columns: new[] { "Kalcode", "Time" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AirQuality",
                table: "AirQuality",
                columns: new[] { "Time", "Kalcode" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Weather",
                table: "Weather");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AirQuality",
                table: "AirQuality");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weather",
                table: "Weather",
                column: "Kalcode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AirQuality",
                table: "AirQuality",
                column: "Kalcode");

            migrationBuilder.CreateIndex(
                name: "IX_Weather_Time_Kalcode",
                table: "Weather",
                columns: new[] { "Time", "Kalcode" });

            migrationBuilder.CreateIndex(
                name: "IX_AirQuality_Time_Kalcode",
                table: "AirQuality",
                columns: new[] { "Time", "Kalcode" });
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace YpenService.Migrations
{
    /// <inheritdoc />
    public partial class CreateGreekRegionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "RegionUnits",
                columns: table => new
                {
                    unit_KALCODE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    unit_Center = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    unit_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    unit_Latitude = table.Column<double>(type: "double precision", nullable: false),
                    unit_Longitude = table.Column<double>(type: "double precision", nullable: false),
                    unit_Shapes = table.Column<Geometry>(type: "geometry(MultiPolygon, 4326)", nullable: false),
                    unit_Area = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionUnits", x => x.unit_KALCODE);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegionUnits");
        }
    }
}

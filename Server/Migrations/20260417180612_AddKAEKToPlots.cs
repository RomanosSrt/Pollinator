using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddKAEKToPlots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Geometry>(
                name: "Polygon",
                table: "Plots",
                type: "geometry(Polygon, 4326)",
                nullable: false,
                oldClrType: typeof(Geometry),
                oldType: "geometry");

            migrationBuilder.AddColumn<string>(
                name: "KAEK",
                table: "Plots",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Plots_KAEK",
                table: "Plots",
                column: "KAEK",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Plots_KAEK",
                table: "Plots");

            migrationBuilder.DropColumn(
                name: "KAEK",
                table: "Plots");

            migrationBuilder.AlterColumn<Geometry>(
                name: "Polygon",
                table: "Plots",
                type: "geometry",
                nullable: false,
                oldClrType: typeof(Geometry),
                oldType: "geometry(Polygon, 4326)");
        }
    }
}

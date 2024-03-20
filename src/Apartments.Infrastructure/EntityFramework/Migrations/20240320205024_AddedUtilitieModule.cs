using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apartments.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddedUtilitieModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UtilityMeasurements",
                columns: table => new
                {
                    ApartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeasurementDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UtilityType = table.Column<int>(type: "int", nullable: false),
                    UtilityUsage = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilityMeasurements", x => new { x.ApartmentId, x.MeasurementDate, x.UtilityType });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UtilityMeasurements");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apartments.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAparmtentAddressTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApartmentAddressDbModel",
                table: "ApartmentAddressDbModel");

            migrationBuilder.RenameTable(
                name: "ApartmentAddressDbModel",
                newName: "ApartmentAddresses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApartmentAddresses",
                table: "ApartmentAddresses",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApartmentAddresses",
                table: "ApartmentAddresses");

            migrationBuilder.RenameTable(
                name: "ApartmentAddresses",
                newName: "ApartmentAddressDbModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApartmentAddressDbModel",
                table: "ApartmentAddressDbModel",
                column: "Id");
        }
    }
}

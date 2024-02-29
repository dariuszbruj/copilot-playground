using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apartments.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddedAddressMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Apartments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_AddressId",
                table: "Apartments",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_ApartmentAddresses_AddressId",
                table: "Apartments",
                column: "AddressId",
                principalTable: "ApartmentAddresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_ApartmentAddresses_AddressId",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_AddressId",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Apartments");
        }
    }
}

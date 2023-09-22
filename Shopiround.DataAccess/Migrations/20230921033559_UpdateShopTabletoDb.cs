using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopiround.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShopTabletoDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerPhoneNo",
                table: "Shops",
                newName: "OpenningTime");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Shops",
                newName: "ImageURL");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ClosingTime",
                table: "Shops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Friday",
                table: "Shops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Monday",
                table: "Shops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Saturday",
                table: "Shops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Sunday",
                table: "Shops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Thursday",
                table: "Shops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Tuesday",
                table: "Shops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Wednesday",
                table: "Shops",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "ClosingTime",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Friday",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Monday",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Saturday",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Sunday",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Thursday",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Tuesday",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "Wednesday",
                table: "Shops");

            migrationBuilder.RenameColumn(
                name: "OpenningTime",
                table: "Shops",
                newName: "OwnerPhoneNo");

            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "Shops",
                newName: "Location");
        }
    }
}

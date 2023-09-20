using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopiround.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShopTableInDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingTime",
                table: "Shops");

            migrationBuilder.DropColumn(
                name: "OpenningTime",
                table: "Shops");

            migrationBuilder.RenameColumn(
                name: "OpenDays",
                table: "Shops",
                newName: "Location");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Shops",
                newName: "OpenDays");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "ClosingTime",
                table: "Shops",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "OpenningTime",
                table: "Shops",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}

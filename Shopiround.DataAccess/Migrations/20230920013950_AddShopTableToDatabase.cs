using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopiround.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddShopTableToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    ShopId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShopPhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnerPhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Upazila = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    OpenningTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    ClosingTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    OpenDays = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcceptOnlineOrders = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.ShopId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shops");
        }
    }
}

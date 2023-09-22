using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopiround.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShopTabletoDb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenningTime",
                table: "Shops",
                newName: "OpeningTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpeningTime",
                table: "Shops",
                newName: "OpenningTime");
        }
    }
}

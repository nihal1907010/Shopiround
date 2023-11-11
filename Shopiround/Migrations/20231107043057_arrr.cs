using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopiround.Migrations
{
    /// <inheritdoc />
    public partial class arrr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "productId",
                table: "DiscountDates",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "productId",
                table: "DiscountDates");
        }
    }
}

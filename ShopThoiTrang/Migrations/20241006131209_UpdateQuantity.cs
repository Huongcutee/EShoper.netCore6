using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopThoiTrang.Migrations
{
    public partial class UpdateQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quanity",
                table: "Products",
                newName: "Quantity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Products",
                newName: "Quanity");
        }
    }
}

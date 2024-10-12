using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopThoiTrang.Migrations
{
    public partial class DatabaseOrderDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "OrderDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

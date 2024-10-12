using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopThoiTrang.Migrations
{
    public partial class update_dataOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "District",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Ward",
                table: "OrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ward",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "District",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Ward",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ward",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

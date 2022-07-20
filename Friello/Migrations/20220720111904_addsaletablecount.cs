using Microsoft.EntityFrameworkCore.Migrations;

namespace Friello.Migrations
{
    public partial class addsaletablecount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "SalesProducts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "SalesProducts");

            migrationBuilder.InsertData(
                table: "Bios",
                columns: new[] { "Id", "AuthorName", "Facebook", "ImageUrl", "Linkedin" },
                values: new object[] { 1, "Samir", "facebook.com", "favicon.png", "linkedin.com" });
        }
    }
}

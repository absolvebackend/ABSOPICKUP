using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class SeedInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "ID", "Type" },
                values: new object[] { 1, "Individual" });

            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "ID", "Type" },
                values: new object[] { 2, "Business" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "ID",
                keyValue: 2);
        }
    }
}

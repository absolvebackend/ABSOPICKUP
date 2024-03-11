using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class AddedAdminUserTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserTypes",
                columns: new[] { "ID", "Type" },
                values: new object[] { 3, "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserTypes",
                keyColumn: "ID",
                keyValue: 3);
        }
    }
}

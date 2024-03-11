using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class AddedDocTypeDriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "DocTypeId", "DocTypeDescription" },
                values: new object[] { 12, "DriverProfilePic" });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "DocTypeId", "DocTypeDescription" },
                values: new object[] { 13, "Any ID Proof" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DocumentTypes",
                keyColumn: "DocTypeId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "DocumentTypes",
                keyColumn: "DocTypeId",
                keyValue: 13);
        }
    }
}

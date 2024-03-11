using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class ChangedCols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "DocTypeId", "DocTypeDescription" },
                values: new object[,]
                {
                    { 18, "IDCardBack" },
                    { 19, "VAT File" },
                    { 20, "ChamberOfCommerce File" },
                    { 21, "Business License File" },
                    { 22, "Agreement File" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DocumentTypes",
                keyColumn: "DocTypeId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "DocumentTypes",
                keyColumn: "DocTypeId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "DocumentTypes",
                keyColumn: "DocTypeId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "DocumentTypes",
                keyColumn: "DocTypeId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "DocumentTypes",
                keyColumn: "DocTypeId",
                keyValue: 22);
        }
    }
}

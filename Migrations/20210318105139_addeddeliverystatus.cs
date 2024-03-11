using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class addeddeliverystatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DeliveryStatus",
                columns: new[] { "StatusId", "Description" },
                values: new object[] { 7, "Cancelled" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DeliveryStatus",
                keyColumn: "StatusId",
                keyValue: 7);
        }
    }
}

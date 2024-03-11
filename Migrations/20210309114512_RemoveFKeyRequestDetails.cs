using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class RemoveFKeyRequestDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryDetails_DriverDetails_DriverId",
                table: "DeliveryDetails");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryDetails_DriverId",
                table: "DeliveryDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDetails_DriverId",
                table: "DeliveryDetails",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryDetails_DriverDetails_DriverId",
                table: "DeliveryDetails",
                column: "DriverId",
                principalTable: "DriverDetails",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

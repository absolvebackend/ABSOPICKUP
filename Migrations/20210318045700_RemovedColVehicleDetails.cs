using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class RemovedColVehicleDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleDetails_DeliveryTypes_DeliveryTypeId",
                table: "VehicleDetails");

            migrationBuilder.DropIndex(
                name: "IX_VehicleDetails_DeliveryTypeId",
                table: "VehicleDetails");

            migrationBuilder.DropColumn(
                name: "DeliveryTypeId",
                table: "VehicleDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryTypeId",
                table: "VehicleDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDetails_DeliveryTypeId",
                table: "VehicleDetails",
                column: "DeliveryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleDetails_DeliveryTypes_DeliveryTypeId",
                table: "VehicleDetails",
                column: "DeliveryTypeId",
                principalTable: "DeliveryTypes",
                principalColumn: "DeliveryTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

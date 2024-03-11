using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class changedColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryRequest_DeliveryTypes_TypeId",
                table: "DeliveryRequest");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "DeliveryTypes",
                newName: "DeliveryTypeId");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "DeliveryRequest",
                newName: "DeliveryTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryRequest_TypeId",
                table: "DeliveryRequest",
                newName: "IX_DeliveryRequest_DeliveryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryRequest_DeliveryTypes_DeliveryTypeId",
                table: "DeliveryRequest",
                column: "DeliveryTypeId",
                principalTable: "DeliveryTypes",
                principalColumn: "DeliveryTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryRequest_DeliveryTypes_DeliveryTypeId",
                table: "DeliveryRequest");

            migrationBuilder.RenameColumn(
                name: "DeliveryTypeId",
                table: "DeliveryTypes",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "DeliveryTypeId",
                table: "DeliveryRequest",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryRequest_DeliveryTypeId",
                table: "DeliveryRequest",
                newName: "IX_DeliveryRequest_TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryRequest_DeliveryTypes_TypeId",
                table: "DeliveryRequest",
                column: "TypeId",
                principalTable: "DeliveryTypes",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class UpdatedRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryRequest_ParcelDetails_ParcelId",
                table: "DeliveryRequest");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryRequest_ParcelId",
                table: "DeliveryRequest");

            migrationBuilder.DropColumn(
                name: "ParcelId",
                table: "DeliveryRequest");

            migrationBuilder.AlterColumn<string>(
                name: "TotalDeliveryTime",
                table: "DeliveryRequest",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldPrecision: 5,
                oldScale: 3);

            migrationBuilder.AlterColumn<string>(
                name: "TotalDeliveryDistance",
                table: "DeliveryRequest",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldPrecision: 5,
                oldScale: 3);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverName",
                table: "DeliveryRequest",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverMobileNumber",
                table: "DeliveryRequest",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AddColumn<string>(
                name: "ReceiverPlaceId",
                table: "DeliveryRequest",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderPlaceId",
                table: "DeliveryRequest",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverPlaceId",
                table: "DeliveryRequest");

            migrationBuilder.DropColumn(
                name: "SenderPlaceId",
                table: "DeliveryRequest");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDeliveryTime",
                table: "DeliveryRequest",
                type: "decimal(5,3)",
                precision: 5,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDeliveryDistance",
                table: "DeliveryRequest",
                type: "decimal(5,3)",
                precision: 5,
                scale: 3,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverName",
                table: "DeliveryRequest",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverMobileNumber",
                table: "DeliveryRequest",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParcelId",
                table: "DeliveryRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRequest_ParcelId",
                table: "DeliveryRequest",
                column: "ParcelId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryRequest_ParcelDetails_ParcelId",
                table: "DeliveryRequest",
                column: "ParcelId",
                principalTable: "ParcelDetails",
                principalColumn: "ParcelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

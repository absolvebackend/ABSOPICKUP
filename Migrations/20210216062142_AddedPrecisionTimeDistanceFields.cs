using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class AddedPrecisionTimeDistanceFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDeliveryTime",
                table: "DeliveryRequest",
                type: "decimal(5,3)",
                precision: 5,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDeliveryDistance",
                table: "DeliveryRequest",
                type: "decimal(5,3)",
                precision: 5,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDeliveryTime",
                table: "DeliveryRequest",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldPrecision: 5,
                oldScale: 3);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDeliveryDistance",
                table: "DeliveryRequest",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,3)",
                oldPrecision: 5,
                oldScale: 3);
        }
    }
}

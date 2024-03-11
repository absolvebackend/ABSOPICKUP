using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class AddedTypeIdColumnBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehicleTypeId",
                table: "VehicleBrand",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 1,
                column: "VehicleTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 2,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 3,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 4,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 5,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 6,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 7,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 8,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 9,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 10,
                column: "VehicleTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 11,
                column: "VehicleTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 12,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 13,
                column: "VehicleTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 14,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 15,
                column: "VehicleTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 16,
                column: "VehicleTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 17,
                column: "VehicleTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 18,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 19,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 20,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 21,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 22,
                column: "VehicleTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 23,
                column: "VehicleTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 24,
                column: "VehicleTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 25,
                column: "VehicleTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 26,
                column: "VehicleTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 27,
                column: "VehicleTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 28,
                column: "VehicleTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 29,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 30,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 31,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 32,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 33,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 34,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 35,
                column: "VehicleTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 36,
                column: "VehicleTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 37,
                column: "VehicleTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 38,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 39,
                column: "VehicleTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 40,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 41,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 42,
                column: "VehicleTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 43,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 44,
                column: "VehicleTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 45,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 46,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 47,
                column: "VehicleTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 48,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 49,
                column: "VehicleTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 50,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 51,
                column: "VehicleTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 52,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 53,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 54,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 55,
                column: "VehicleTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "VehicleBrand",
                keyColumn: "BrandId",
                keyValue: 56,
                column: "VehicleTypeId",
                value: 3);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleTypeId",
                table: "VehicleBrand");
        }
    }
}

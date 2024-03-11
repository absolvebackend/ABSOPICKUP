using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class addedColorCodeCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VehicleColorCode",
                table: "VehicleColour",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 1,
                column: "VehicleColorCode",
                value: "#ffffff");

            migrationBuilder.UpdateData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 2,
                column: "VehicleColorCode",
                value: "#c0c0c0");

            migrationBuilder.UpdateData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 3,
                column: "VehicleColorCode",
                value: "#808080");

            migrationBuilder.UpdateData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 4,
                column: "VehicleColorCode",
                value: "#ff3b30");

            migrationBuilder.UpdateData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 5,
                column: "VehicleColorCode",
                value: "#007aff");

            migrationBuilder.UpdateData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 6,
                column: "VehicleColorCode",
                value: "#f5f5dc");

            migrationBuilder.UpdateData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 7,
                column: "VehicleColorCode",
                value: "#ff9500");

            migrationBuilder.UpdateData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 8,
                column: "VehicleColorCode",
                value: "#ffd700");

            migrationBuilder.UpdateData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 9,
                column: "VehicleColorCode",
                value: "#000000");

            migrationBuilder.UpdateData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 10,
                column: "VehicleColorCode",
                value: "#4cd964");

            migrationBuilder.InsertData(
                table: "VehicleColour",
                columns: new[] { "VehicleColorId", "VehicleColorCode", "VehicleColorName" },
                values: new object[,]
                {
                    { 11, "#ffcc00", "Yellow" },
                    { 12, "#5ac8fa", "Teal Blue" },
                    { 13, "#5856d6", "Purple" },
                    { 14, "#ff2d55", "Pink" },
                    { 15, "#a52a2a", "Brown" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 15);

            migrationBuilder.DropColumn(
                name: "VehicleColorCode",
                table: "VehicleColour");
        }
    }
}

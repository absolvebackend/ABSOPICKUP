using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class addedNewVehicleColors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "VehicleColour",
                columns: new[] { "VehicleColorId", "VehicleColorCode", "VehicleColorName" },
                values: new object[,]
                {
                    { 16, "#999da0", "Metallic Silver" },
                    { 42, "#5870a1", "Chrome" },
                    { 41, "#9ba0a8", "Brushed Aluminium" },
                    { 40, "#6a747c", "Brushed Steel" },
                    { 39, "#453831", "Worn Brown" },
                    { 38, "#fffff6", "Metallic White" },
                    { 37, "#f7edd5", "Metallic Cream" },
                    { 36, "#dfd5b2", "Metallic Sun Bleeched Sand" },
                    { 35, "#a4965f", "Metallic Beechwood" },
                    { 34, "#775c3e", "Metallic Light Brown" },
                    { 33, "#473f2b", "Metallic Dark Ivory" },
                    { 32, "#9b8c78", "Metallic Champagne" },
                    { 31, "#98d223", "Metallic Lime" },
                    { 43, "#bcac8f", "Matte Brown" },
                    { 30, "#ffcf20", "Metallic Taxi Yellow" },
                    { 28, "#65867f", "Worn Sea Wash" },
                    { 27, "#155c2d", "Metallic Green" },
                    { 26, "#ffc91f", "Matte Yellow" },
                    { 25, "#f78616", "Metallic Orange" },
                    { 24, "#c2944f", "Metallic Classic Gold" },
                    { 23, "#c00e1a", "Metallic Red" },
                    { 22, "#d3d3d3", "Worn Silver" },
                    { 21, "#363a3f", "Worn Graphite" },
                    { 20, "#151921", "Util Black" },
                    { 19, "#515554", "Matte Light Grey" },
                    { 18, "#26282a", "Matte Gray" },
                    { 17, "#444e54", "Metallic Gun Metal" },
                    { 29, "#47578f", "Metallic Blue" },
                    { 44, "#6b1f7b", "Matte Purple" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "VehicleColour",
                keyColumn: "VehicleColorId",
                keyValue: 44);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class AddedProvincesUpdatedDriverDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "DriverDetails",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DOB",
                table: "DriverDetails",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProvinceId",
                table: "DriverDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SouthAfricaProvinces",
                columns: table => new
                {
                    ProvinceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SouthAfricaProvinces", x => x.ProvinceId);
                });

            migrationBuilder.InsertData(
                table: "SouthAfricaProvinces",
                columns: new[] { "ProvinceId", "Name" },
                values: new object[,]
                {
                    { 1, "Eastern Cape" },
                    { 2, "Free State" },
                    { 3, "Gauteng" },
                    { 4, "KwaZulu-Natal" },
                    { 5, "Limpopo" },
                    { 6, "Mpumalanga" },
                    { 7, "Northern Cape" },
                    { 8, "North West" },
                    { 9, "Western Cape" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverDetails_ProvinceId",
                table: "DriverDetails",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DriverDetails_SouthAfricaProvinces_ProvinceId",
                table: "DriverDetails",
                column: "ProvinceId",
                principalTable: "SouthAfricaProvinces",
                principalColumn: "ProvinceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DriverDetails_SouthAfricaProvinces_ProvinceId",
                table: "DriverDetails");

            migrationBuilder.DropTable(
                name: "SouthAfricaProvinces");

            migrationBuilder.DropIndex(
                name: "IX_DriverDetails_ProvinceId",
                table: "DriverDetails");

            migrationBuilder.DropColumn(
                name: "City",
                table: "DriverDetails");

            migrationBuilder.DropColumn(
                name: "DOB",
                table: "DriverDetails");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "DriverDetails");
        }
    }
}

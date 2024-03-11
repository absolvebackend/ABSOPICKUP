using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class AddedAppStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "DriverDetails");

            migrationBuilder.DropColumn(
                name: "ApplicationStatus",
                table: "BusinessDocuments");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "BusinessDetails");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationStatus",
                table: "DriverDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationStatus",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationStatus",
                table: "DriverDetails");

            migrationBuilder.DropColumn(
                name: "ApplicationStatus",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "DriverDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationStatus",
                table: "BusinessDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "BusinessDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

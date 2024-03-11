using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class AddedDialingFieldsUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DialCode",
                table: "AspNetUsers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Otp",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DialCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Otp",
                table: "AspNetUsers");
        }
    }
}

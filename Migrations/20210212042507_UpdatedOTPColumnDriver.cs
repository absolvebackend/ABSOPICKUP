using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class UpdatedOTPColumnDriver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Otp",
                table: "DriverDetails",
                type: "int",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 25);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Otp",
                table: "DriverDetails",
                type: "int",
                maxLength: 25,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 25,
                oldNullable: true);
        }
    }
}

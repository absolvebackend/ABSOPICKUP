using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class changeColumnBusinessContract : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalNumber",
                table: "BusinessDetails");

            migrationBuilder.AddColumn<string>(
                name: "ExternalContractNumber",
                table: "BusinessDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalContractNumber",
                table: "BusinessDetails");

            migrationBuilder.AddColumn<string>(
                name: "ExternalNumber",
                table: "BusinessDetails",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");
        }
    }
}

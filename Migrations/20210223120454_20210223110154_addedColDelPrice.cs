using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class _20210223110154_addedColDelPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<string>(
                name: "DeliverBy",
                table: "DeliveryPrice",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DeliveryPrice",
                keyColumn: "Id",
                keyValue: 1,
                column: "DeliverBy",
                value: "Same day delivery");

            migrationBuilder.UpdateData(
                table: "DeliveryPrice",
                keyColumn: "Id",
                keyValue: 2,
                column: "DeliverBy",
                value: "Within 2 hrs delivery");

            migrationBuilder.UpdateData(
                table: "DeliveryPrice",
                keyColumn: "Id",
                keyValue: 3,
                column: "DeliverBy",
                value: "Immediate delivery");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliverBy",
                table: "DeliveryPrice");

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

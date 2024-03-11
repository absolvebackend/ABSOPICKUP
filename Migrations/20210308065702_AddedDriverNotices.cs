using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class AddedDriverNotices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverRatings",
                columns: table => new
                {
                    RatingsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    Ratings = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverRatings", x => x.RatingsId);
                });

            migrationBuilder.CreateTable(
                name: "ParcelNotifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    DriverLat = table.Column<decimal>(type: "decimal(18,9)", precision: 18, scale: 9, nullable: false),
                    DriverLon = table.Column<decimal>(type: "decimal(18,9)", precision: 18, scale: 9, nullable: false),
                    DriverPlaceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    Accepted = table.Column<bool>(type: "bit", nullable: false),
                    IsNotificationSent = table.Column<bool>(type: "bit", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NotifySentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelNotifications", x => x.NotificationId);
                });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "DocTypeId", "DocTypeDescription" },
                values: new object[] { 16, "Vehicle Frontside Image" });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "DocTypeId", "DocTypeDescription" },
                values: new object[] { 17, "Vehicle Backside Image" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverRatings");

            migrationBuilder.DropTable(
                name: "ParcelNotifications");

            migrationBuilder.DeleteData(
                table: "DocumentTypes",
                keyColumn: "DocTypeId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "DocumentTypes",
                keyColumn: "DocTypeId",
                keyValue: 17);
        }
    }
}

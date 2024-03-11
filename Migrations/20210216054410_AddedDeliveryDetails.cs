using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class AddedDeliveryDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryRequest",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderLat = table.Column<decimal>(type: "decimal(18,9)", precision: 18, scale: 9, nullable: false),
                    SenderLong = table.Column<decimal>(type: "decimal(18,9)", precision: 18, scale: 9, nullable: false),
                    ReceiverName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DialCode = table.Column<int>(type: "int", nullable: false),
                    ReceiverMobileNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ReceiverLat = table.Column<decimal>(type: "decimal(18,9)", precision: 18, scale: 9, nullable: false),
                    ReceiverLong = table.Column<decimal>(type: "decimal(18,9)", precision: 18, scale: 9, nullable: false),
                    PackageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    TotalDeliveryTime = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDeliveryDistance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryRequest", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_DeliveryRequest_DeliveryTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DeliveryTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryStatus",
                columns: table => new
                {
                    StatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryStatus", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryDetails",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    RequestId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    DriverLat = table.Column<decimal>(type: "decimal(18,9)", precision: 18, scale: 9, nullable: false),
                    DriverLong = table.Column<decimal>(type: "decimal(18,9)", precision: 18, scale: 9, nullable: false),
                    DeliveryDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryDetails", x => x.DeliveryId);
                    table.ForeignKey(
                        name: "FK_DeliveryDetails_DeliveryRequest_RequestId",
                        column: x => x.RequestId,
                        principalTable: "DeliveryRequest",
                        principalColumn: "RequestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryDetails_DeliveryStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "DeliveryStatus",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryDetails_DriverDetails_DriverId",
                        column: x => x.DriverId,
                        principalTable: "DriverDetails",
                        principalColumn: "DriverId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DeliveryStatus",
                columns: new[] { "StatusId", "Description" },
                values: new object[,]
                {
                    { 1, "Unassigned" },
                    { 2, "Assigned" },
                    { 3, "With Driver" },
                    { 4, "Delivery on Route" },
                    { 5, "Arrived" },
                    { 6, "Delivered" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDetails_DriverId",
                table: "DeliveryDetails",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDetails_RequestId",
                table: "DeliveryDetails",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDetails_StatusId",
                table: "DeliveryDetails",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRequest_TypeId",
                table: "DeliveryRequest",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryDetails");

            migrationBuilder.DropTable(
                name: "DeliveryRequest");

            migrationBuilder.DropTable(
                name: "DeliveryStatus");
        }
    }
}

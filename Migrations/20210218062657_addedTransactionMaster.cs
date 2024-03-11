using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class addedTransactionMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "PaymentDetails",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "PaymentDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TransactionMaster",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PaymentStatusId = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    TransactionType = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionMaster_PaymentStatus_PaymentStatusId",
                        column: x => x.PaymentStatusId,
                        principalTable: "PaymentStatus",
                        principalColumn: "PaymentStatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PaymentStatus",
                columns: new[] { "PaymentStatusId", "PaymentStatusName" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Success" },
                    { 3, "Complete" },
                    { 4, "Cancelled" },
                    { 5, "Rejected" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionMaster_PaymentStatusId",
                table: "TransactionMaster",
                column: "PaymentStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionMaster");

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PaymentStatus",
                keyColumn: "PaymentStatusId",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "PaymentDetails");
        }
    }
}

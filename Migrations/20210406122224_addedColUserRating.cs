using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class addedColUserRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDetails_DeliveryRequest_RequestId",
                table: "PaymentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDetails_PaymentStatus_PaymentStatusId",
                table: "PaymentDetails");

            migrationBuilder.DropTable(
                name: "PaymentStatus");

            migrationBuilder.DropIndex(
                name: "IX_TransactionMaster_PaymentStatusId",
                table: "TransactionMaster");

            migrationBuilder.DropIndex(
                name: "IX_PaymentDetails_PaymentStatusId",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "PaymentStatusId",
                table: "TransactionMaster");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "TransactionMaster",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "PaymentDetails",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentDetails_RequestId",
                table: "PaymentDetails",
                newName: "IX_PaymentDetails_OrderId");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentMode",
                table: "TransactionMaster",
                type: "int",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TransactionMaster",
                type: "int",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PaymentDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PaymentDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CommentedAt",
                table: "DriverRatings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetails_Orders_OrderId",
                table: "PaymentDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDetails_Orders_OrderId",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "CommentedAt",
                table: "DriverRatings");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "TransactionMaster",
                newName: "RequestId");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "PaymentDetails",
                newName: "RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentDetails_OrderId",
                table: "PaymentDetails",
                newName: "IX_PaymentDetails_RequestId");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMode",
                table: "TransactionMaster",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "TransactionMaster",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatusId",
                table: "TransactionMaster",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PaymentStatus",
                columns: table => new
                {
                    PaymentStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentStatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatus", x => x.PaymentStatusId);
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

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_PaymentStatusId",
                table: "PaymentDetails",
                column: "PaymentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetails_DeliveryRequest_RequestId",
                table: "PaymentDetails",
                column: "RequestId",
                principalTable: "DeliveryRequest",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetails_PaymentStatus_PaymentStatusId",
                table: "PaymentDetails",
                column: "PaymentStatusId",
                principalTable: "PaymentStatus",
                principalColumn: "PaymentStatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionMaster_PaymentStatus_PaymentStatusId",
                table: "TransactionMaster",
                column: "PaymentStatusId",
                principalTable: "PaymentStatus",
                principalColumn: "PaymentStatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class ChangedParcelDetailsTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParcelDetails_ParcelCategory_CategoryId",
                table: "ParcelDetails");

            migrationBuilder.DropIndex(
                name: "IX_ParcelDetails_CategoryId",
                table: "ParcelDetails");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ParcelDetails");

            migrationBuilder.DropColumn(
                name: "ImgAfterPackingPathOne",
                table: "ParcelDetails");

            migrationBuilder.DropColumn(
                name: "ImgAfterPackingPathTwo",
                table: "ParcelDetails");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "ParcelDetails");

            migrationBuilder.RenameColumn(
                name: "ImgBeforePackingPathTwo",
                table: "ParcelDetails",
                newName: "ImgBeforePacking");

            migrationBuilder.RenameColumn(
                name: "ImgBeforePackingPathOne",
                table: "ParcelDetails",
                newName: "ImgAfterPacking");

            migrationBuilder.AddColumn<string>(
                name: "ParcelNotes",
                table: "ParcelDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParcelNotes",
                table: "ParcelDetails");

            migrationBuilder.RenameColumn(
                name: "ImgBeforePacking",
                table: "ParcelDetails",
                newName: "ImgBeforePackingPathTwo");

            migrationBuilder.RenameColumn(
                name: "ImgAfterPacking",
                table: "ParcelDetails",
                newName: "ImgBeforePackingPathOne");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ParcelDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImgAfterPackingPathOne",
                table: "ParcelDetails",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImgAfterPackingPathTwo",
                table: "ParcelDetails",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "ParcelDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ParcelDetails_CategoryId",
                table: "ParcelDetails",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParcelDetails_ParcelCategory_CategoryId",
                table: "ParcelDetails",
                column: "CategoryId",
                principalTable: "ParcelCategory",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

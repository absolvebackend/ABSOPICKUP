using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class AddedParcelTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageName",
                table: "DeliveryRequest");

            migrationBuilder.AddColumn<int>(
                name: "ParcelId",
                table: "DeliveryRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ParcelCategory",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelCategory", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ParcelDetails",
                columns: table => new
                {
                    ParcelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false),
                    ParcelName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ImgBeforePackingPathOne = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImgBeforePackingPathTwo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImgAfterPackingPathOne = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImgAfterPackingPathTwo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelDetails", x => x.ParcelId);
                    table.ForeignKey(
                        name: "FK_ParcelDetails_ParcelCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ParcelCategory",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParcelSubCategory",
                columns: table => new
                {
                    SubCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelSubCategory", x => x.SubCategoryId);
                    table.ForeignKey(
                        name: "FK_ParcelSubCategory_ParcelCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ParcelCategory",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "DocTypeId", "DocTypeDescription" },
                values: new object[] { 14, "Image Before Packing" });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "DocTypeId", "DocTypeDescription" },
                values: new object[] { 15, "Image After Packing" });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryRequest_ParcelId",
                table: "DeliveryRequest",
                column: "ParcelId");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelDetails_CategoryId",
                table: "ParcelDetails",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelSubCategory_CategoryId",
                table: "ParcelSubCategory",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryRequest_ParcelDetails_ParcelId",
                table: "DeliveryRequest",
                column: "ParcelId",
                principalTable: "ParcelDetails",
                principalColumn: "ParcelId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryRequest_ParcelDetails_ParcelId",
                table: "DeliveryRequest");

            migrationBuilder.DropTable(
                name: "ParcelDetails");

            migrationBuilder.DropTable(
                name: "ParcelSubCategory");

            migrationBuilder.DropTable(
                name: "ParcelCategory");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryRequest_ParcelId",
                table: "DeliveryRequest");

            migrationBuilder.DeleteData(
                table: "DocumentTypes",
                keyColumn: "DocTypeId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "DocumentTypes",
                keyColumn: "DocTypeId",
                keyValue: 15);

            migrationBuilder.DropColumn(
                name: "ParcelId",
                table: "DeliveryRequest");

            migrationBuilder.AddColumn<string>(
                name: "PackageName",
                table: "DeliveryRequest",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}

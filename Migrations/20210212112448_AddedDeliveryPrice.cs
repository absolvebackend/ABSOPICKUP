using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class AddedDeliveryPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryPrice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPrice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryPrice_DeliveryTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DeliveryTypes",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DeliveryPrice",
                columns: new[] { "Id", "Amount", "TypeId" },
                values: new object[] { 1, 45, 1 });

            migrationBuilder.InsertData(
                table: "DeliveryPrice",
                columns: new[] { "Id", "Amount", "TypeId" },
                values: new object[] { 2, 80, 2 });

            migrationBuilder.InsertData(
                table: "DeliveryPrice",
                columns: new[] { "Id", "Amount", "TypeId" },
                values: new object[] { 3, 300, 3 });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPrice_TypeId",
                table: "DeliveryPrice",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryPrice");
        }
    }
}

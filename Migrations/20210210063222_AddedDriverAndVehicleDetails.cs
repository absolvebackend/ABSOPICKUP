using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace _AbsoPickUp.Migrations
{
    public partial class AddedDriverAndVehicleDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryTypes",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryTypes", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    DocTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocTypeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.DocTypeId);
                });

            migrationBuilder.CreateTable(
                name: "DriverDetails",
                columns: table => new
                {
                    DriverId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Otp = table.Column<int>(type: "int", maxLength: 25, nullable: false),
                    DialCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DeviceToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePic = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverDetails", x => x.DriverId);
                });

            migrationBuilder.CreateTable(
                name: "VehicleBrand",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleBrand", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "VehicleColour",
                columns: table => new
                {
                    VehicleColorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleColorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleColour", x => x.VehicleColorId);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.VehicleTypeId);
                });

            migrationBuilder.CreateTable(
                name: "DriverDocuments",
                columns: table => new
                {
                    DocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocTypeId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverDocuments", x => x.DocId);
                    table.ForeignKey(
                        name: "FK_DriverDocuments_DocumentTypes_DocTypeId",
                        column: x => x.DocTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "DocTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndividualUserDocuments",
                columns: table => new
                {
                    DocId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocTypeId = table.Column<int>(type: "int", nullable: false),
                    IndividualUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualUserDocuments", x => x.DocId);
                    table.ForeignKey(
                        name: "FK_IndividualUserDocuments_DocumentTypes_DocTypeId",
                        column: x => x.DocTypeId,
                        principalTable: "DocumentTypes",
                        principalColumn: "DocTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleDetails",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleTypeId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    VehicleColorId = table.Column<int>(type: "int", nullable: false),
                    RegisterationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleDetails", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_VehicleDetails_DriverDetails_DriverId",
                        column: x => x.DriverId,
                        principalTable: "DriverDetails",
                        principalColumn: "DriverId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleDetails_VehicleBrand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "VehicleBrand",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleDetails_VehicleColour_VehicleColorId",
                        column: x => x.VehicleColorId,
                        principalTable: "VehicleColour",
                        principalColumn: "VehicleColorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleDetails_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "VehicleTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DeliveryTypes",
                columns: new[] { "TypeId", "Description" },
                values: new object[,]
                {
                    { 1, "Normal" },
                    { 2, "Express" },
                    { 3, "Bakkie" }
                });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "DocTypeId", "DocTypeDescription" },
                values: new object[,]
                {
                    { 9, "BirthCertificate" },
                    { 8, "ElectionCard" },
                    { 7, "Selfie" },
                    { 6, "IDCard" },
                    { 10, "Passport" },
                    { 4, "VehicleRegisteration" },
                    { 3, "NumberPlatePic" },
                    { 2, "ProofOfResidence" },
                    { 1, "License" },
                    { 5, "IDBook" }
                });

            migrationBuilder.InsertData(
                table: "VehicleBrand",
                columns: new[] { "BrandId", "BrandName" },
                values: new object[,]
                {
                    { 41, "Mitsubishi" },
                    { 40, "Opel" },
                    { 39, "Bajaj" },
                    { 38, "Citroen" },
                    { 37, "Leyland" },
                    { 34, "Chrysler" },
                    { 35, "Isuzu" },
                    { 33, "Vauxhall" },
                    { 32, "Volvo" },
                    { 31, "Hummer" },
                    { 42, "Jeep" },
                    { 36, "Nissan" },
                    { 43, "Peugeot" },
                    { 49, "Daihatsu" },
                    { 45, "Porsche" },
                    { 46, "Datsun" },
                    { 47, "Daewoo" },
                    { 48, "Kia" },
                    { 30, "Chevrolet" },
                    { 50, "Renault" },
                    { 51, "Piaggio" },
                    { 52, "Daimler" },
                    { 53, "Rover" },
                    { 54, "Austin" },
                    { 55, "Saab" },
                    { 56, "Willys" },
                    { 44, "Kawasaki" },
                    { 29, "Volkswagen" },
                    { 28, "Hyundai" }
                });

            migrationBuilder.InsertData(
                table: "VehicleBrand",
                columns: new[] { "BrandId", "BrandName" },
                values: new object[,]
                {
                    { 27, "Yamaha" },
                    { 1, "GWM" },
                    { 2, "Ferrari" },
                    { 3, "Lexus" },
                    { 4, "Austin-Healey" },
                    { 5, "Alfa Romeo" },
                    { 6, "Fiat" },
                    { 7, "Aston Martin" },
                    { 9, "Audi" },
                    { 10, "Ford" },
                    { 11, "Mahindra" },
                    { 12, "Subaru" },
                    { 13, "Hero" },
                    { 8, "Maserati" },
                    { 15, "Land Cruiser" },
                    { 14, "BMW" },
                    { 25, "Packard" },
                    { 24, "Pontiac" },
                    { 23, "Toyota" },
                    { 22, "Harley-Davidson" },
                    { 21, "Cadillac" },
                    { 26, "Honda" },
                    { 19, "Tata" },
                    { 18, "Mazda" },
                    { 17, "TVS" },
                    { 16, "Suzuki" },
                    { 20, "KTM" }
                });

            migrationBuilder.InsertData(
                table: "VehicleColour",
                columns: new[] { "VehicleColorId", "VehicleColorName" },
                values: new object[,]
                {
                    { 4, "Red" },
                    { 1, "White" },
                    { 2, "Silver" },
                    { 3, "Gray" },
                    { 5, "Blue" },
                    { 6, "Beige" },
                    { 7, "Orange" },
                    { 8, "Gold" },
                    { 9, "Black" },
                    { 10, "Green" }
                });

            migrationBuilder.InsertData(
                table: "VehicleTypes",
                columns: new[] { "VehicleTypeId", "VehicleType" },
                values: new object[,]
                {
                    { 2, "Car" },
                    { 1, "Scooter" },
                    { 3, "Bakkie" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverDocuments_DocTypeId",
                table: "DriverDocuments",
                column: "DocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualUserDocuments_DocTypeId",
                table: "IndividualUserDocuments",
                column: "DocTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDetails_BrandId",
                table: "VehicleDetails",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDetails_DriverId",
                table: "VehicleDetails",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDetails_VehicleColorId",
                table: "VehicleDetails",
                column: "VehicleColorId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleDetails_VehicleTypeId",
                table: "VehicleDetails",
                column: "VehicleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryTypes");

            migrationBuilder.DropTable(
                name: "DriverDocuments");

            migrationBuilder.DropTable(
                name: "IndividualUserDocuments");

            migrationBuilder.DropTable(
                name: "VehicleDetails");

            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.DropTable(
                name: "DriverDetails");

            migrationBuilder.DropTable(
                name: "VehicleBrand");

            migrationBuilder.DropTable(
                name: "VehicleColour");

            migrationBuilder.DropTable(
                name: "VehicleTypes");
        }
    }
}

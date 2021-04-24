using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Farmacio_API.Migrations
{
    public partial class Addedpharmacyorders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PharmacyOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    PharmacyId = table.Column<Guid>(nullable: false),
                    PharmacyAdminId = table.Column<Guid>(nullable: false),
                    OffersDeadline = table.Column<DateTime>(nullable: false),
                    IsProcessed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmacyOrders_Users_PharmacyAdminId",
                        column: x => x.PharmacyAdminId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PharmacyOrders_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderedMedicine",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: false),
                    PharmacyOrderId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderedMedicine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderedMedicine_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderedMedicine_PharmacyOrders_PharmacyOrderId",
                        column: x => x.PharmacyOrderId,
                        principalTable: "PharmacyOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderedMedicine_MedicineId",
                table: "OrderedMedicine",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedMedicine_PharmacyOrderId",
                table: "OrderedMedicine",
                column: "PharmacyOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyOrders_PharmacyAdminId",
                table: "PharmacyOrders",
                column: "PharmacyAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyOrders_PharmacyId",
                table: "PharmacyOrders",
                column: "PharmacyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderedMedicine");

            migrationBuilder.DropTable(
                name: "PharmacyOrders");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Farmacio_API.Migrations
{
    public partial class addedstock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Pharmacies_PharmacyId",
                table: "Reservations");

            migrationBuilder.AlterColumn<Guid>(
                name: "PharmacyId",
                table: "Reservations",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PharmacyMedicine",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    PharmacyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyMedicine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmacyMedicine_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PharmacyMedicine_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyMedicine_MedicineId",
                table: "PharmacyMedicine",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyMedicine_PharmacyId",
                table: "PharmacyMedicine",
                column: "PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Pharmacies_PharmacyId",
                table: "Reservations",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Pharmacies_PharmacyId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "PharmacyMedicine");

            migrationBuilder.AlterColumn<Guid>(
                name: "PharmacyId",
                table: "Reservations",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Pharmacies_PharmacyId",
                table: "Reservations",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

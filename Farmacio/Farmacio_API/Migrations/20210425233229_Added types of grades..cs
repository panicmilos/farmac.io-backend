using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Farmacio_API.Migrations
{
    public partial class Addedtypesofgrades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicinePrices_PharmacyPriceLists_PharmacyPriceListId",
                table: "MedicinePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedMedicine_PharmacyOrders_PharmacyOrderId",
                table: "OrderedMedicine");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfGrades",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfGrades",
                table: "Pharmacies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "PharmacyOrderId",
                table: "OrderedMedicine",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfGrades",
                table: "Medicines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "PharmacyPriceListId",
                table: "MedicinePrices",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Grades",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "MedicalStaffId",
                table: "Grades",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MedicineId",
                table: "Grades",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PharmacyId",
                table: "Grades",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicinePrices_PharmacyPriceLists_PharmacyPriceListId",
                table: "MedicinePrices",
                column: "PharmacyPriceListId",
                principalTable: "PharmacyPriceLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedMedicine_PharmacyOrders_PharmacyOrderId",
                table: "OrderedMedicine",
                column: "PharmacyOrderId",
                principalTable: "PharmacyOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicinePrices_PharmacyPriceLists_PharmacyPriceListId",
                table: "MedicinePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderedMedicine_PharmacyOrders_PharmacyOrderId",
                table: "OrderedMedicine");

            migrationBuilder.DropColumn(
                name: "NumberOfGrades",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NumberOfGrades",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "NumberOfGrades",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "MedicalStaffId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "MedicineId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "PharmacyId",
                table: "Grades");

            migrationBuilder.AlterColumn<Guid>(
                name: "PharmacyOrderId",
                table: "OrderedMedicine",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "PharmacyPriceListId",
                table: "MedicinePrices",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_MedicinePrices_PharmacyPriceLists_PharmacyPriceListId",
                table: "MedicinePrices",
                column: "PharmacyPriceListId",
                principalTable: "PharmacyPriceLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedMedicine_PharmacyOrders_PharmacyOrderId",
                table: "OrderedMedicine",
                column: "PharmacyOrderId",
                principalTable: "PharmacyOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

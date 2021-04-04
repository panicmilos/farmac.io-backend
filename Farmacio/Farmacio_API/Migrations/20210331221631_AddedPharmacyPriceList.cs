using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Farmacio_API.Migrations
{
    public partial class AddedPharmacyPriceList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PharmacyPriceListId",
                table: "MedicinePrices",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PharmacyPriceLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    PharmacyId = table.Column<Guid>(nullable: false),
                    ExaminationPrice = table.Column<float>(nullable: false),
                    ConsultationPrice = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyPriceLists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePrices_PharmacyPriceListId",
                table: "MedicinePrices",
                column: "PharmacyPriceListId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicinePrices_PharmacyPriceLists_PharmacyPriceListId",
                table: "MedicinePrices",
                column: "PharmacyPriceListId",
                principalTable: "PharmacyPriceLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicinePrices_PharmacyPriceLists_PharmacyPriceListId",
                table: "MedicinePrices");

            migrationBuilder.DropTable(
                name: "PharmacyPriceLists");

            migrationBuilder.DropIndex(
                name: "IX_MedicinePrices_PharmacyPriceListId",
                table: "MedicinePrices");

            migrationBuilder.DropColumn(
                name: "PharmacyPriceListId",
                table: "MedicinePrices");
        }
    }
}

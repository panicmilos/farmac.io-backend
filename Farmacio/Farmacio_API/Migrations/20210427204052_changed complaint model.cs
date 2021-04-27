using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Farmacio_API.Migrations
{
    public partial class changedcomplaintmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAboutPharmacy",
                table: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Complaints",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "DermatologistId",
                table: "Complaints",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PharmacistId",
                table: "Complaints",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PharmacyId",
                table: "Complaints",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_DermatologistId",
                table: "Complaints",
                column: "DermatologistId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_PharmacistId",
                table: "Complaints",
                column: "PharmacistId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_PharmacyId",
                table: "Complaints",
                column: "PharmacyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Users_DermatologistId",
                table: "Complaints",
                column: "DermatologistId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Users_PharmacistId",
                table: "Complaints",
                column: "PharmacistId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Complaints_Pharmacies_PharmacyId",
                table: "Complaints",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Users_DermatologistId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Users_PharmacistId",
                table: "Complaints");

            migrationBuilder.DropForeignKey(
                name: "FK_Complaints_Pharmacies_PharmacyId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_DermatologistId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_PharmacistId",
                table: "Complaints");

            migrationBuilder.DropIndex(
                name: "IX_Complaints_PharmacyId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "DermatologistId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "PharmacistId",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "PharmacyId",
                table: "Complaints");

            migrationBuilder.AddColumn<bool>(
                name: "IsAboutPharmacy",
                table: "Complaints",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}

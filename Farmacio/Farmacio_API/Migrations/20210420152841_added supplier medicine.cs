using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Farmacio_API.Migrations
{
    public partial class addedsuppliermedicine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicineIngredient_Medicines_MedicineId",
                table: "MedicineIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_ERecipes_ERecipeId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_LoyaltyProgram_LoyaltyProgramId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicineIngredient",
                table: "MedicineIngredient");

            migrationBuilder.DropIndex(
                name: "IX_MedicineIngredient_MedicineId",
                table: "MedicineIngredient");

            migrationBuilder.RenameTable(
                name: "MedicineIngredient",
                newName: "MedicineIngredients");

            migrationBuilder.AlterColumn<Guid>(
                name: "ERecipeId",
                table: "Report",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AlterColumn<Guid>(
                name: "MedicineId",
                table: "MedicineIngredients",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicineIngredients",
                table: "MedicineIngredients",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SupplierMedicines",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    SupplierId = table.Column<Guid>(nullable: false),
                    MedicineId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierMedicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierMedicines_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierMedicines_MedicineId",
                table: "SupplierMedicines",
                column: "MedicineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_ERecipes_ERecipeId",
                table: "Report",
                column: "ERecipeId",
                principalTable: "ERecipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LoyaltyProgram_LoyaltyProgramId",
                table: "Users",
                column: "LoyaltyProgramId",
                principalTable: "LoyaltyProgram",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_ERecipes_ERecipeId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_LoyaltyProgram_LoyaltyProgramId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "SupplierMedicines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicineIngredients",
                table: "MedicineIngredients");

            migrationBuilder.RenameTable(
                name: "MedicineIngredients",
                newName: "MedicineIngredient");

            migrationBuilder.AlterColumn<Guid>(
                name: "ERecipeId",
                table: "Report",
                type: "char(36)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "MedicineId",
                table: "MedicineIngredient",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicineIngredient",
                table: "MedicineIngredient",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineIngredient_MedicineId",
                table: "MedicineIngredient",
                column: "MedicineId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineIngredient_Medicines_MedicineId",
                table: "MedicineIngredient",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_ERecipes_ERecipeId",
                table: "Report",
                column: "ERecipeId",
                principalTable: "ERecipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LoyaltyProgram_LoyaltyProgramId",
                table: "Users",
                column: "LoyaltyProgramId",
                principalTable: "LoyaltyProgram",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

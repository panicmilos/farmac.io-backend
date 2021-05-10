using Microsoft.EntityFrameworkCore.Migrations;

namespace Farmacio_API.Migrations
{
    public partial class addedoriginalpricetoappointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "OriginalPrice",
                table: "Appointments",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_NotInStocks_MedicineId",
                table: "NotInStocks",
                column: "MedicineId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotInStocks_Medicines_MedicineId",
                table: "NotInStocks",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotInStocks_Medicines_MedicineId",
                table: "NotInStocks");

            migrationBuilder.DropIndex(
                name: "IX_NotInStocks_MedicineId",
                table: "NotInStocks");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "Appointments");
        }
    }
}

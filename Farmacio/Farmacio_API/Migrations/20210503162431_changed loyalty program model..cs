using Microsoft.EntityFrameworkCore.Migrations;

namespace Farmacio_API.Migrations
{
    public partial class changedloyaltyprogrammodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "LoyaltyProgram");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "LoyaltyProgram",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "LoyaltyProgram");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "LoyaltyProgram",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

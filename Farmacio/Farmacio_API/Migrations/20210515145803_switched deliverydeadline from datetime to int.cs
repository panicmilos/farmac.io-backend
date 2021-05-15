using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Farmacio_API.Migrations
{
    public partial class switcheddeliverydeadlinefromdatetimetoint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DeliveryDeadline",
                table: "SupplierOffers",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DeliveryDeadline",
                table: "SupplierOffers",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
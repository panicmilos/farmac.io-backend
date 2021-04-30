using Microsoft.EntityFrameworkCore.Migrations;

namespace Farmacio_API.Migrations
{
    public partial class addedanswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplaintAnswer_Complaints_ComplaintId",
                table: "ComplaintAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_ComplaintAnswer_Users_WriterId",
                table: "ComplaintAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComplaintAnswer",
                table: "ComplaintAnswer");

            migrationBuilder.RenameTable(
                name: "ComplaintAnswer",
                newName: "ComplaintAnswers");

            migrationBuilder.RenameIndex(
                name: "IX_ComplaintAnswer_WriterId",
                table: "ComplaintAnswers",
                newName: "IX_ComplaintAnswers_WriterId");

            migrationBuilder.RenameIndex(
                name: "IX_ComplaintAnswer_ComplaintId",
                table: "ComplaintAnswers",
                newName: "IX_ComplaintAnswers_ComplaintId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComplaintAnswers",
                table: "ComplaintAnswers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintAnswers_Complaints_ComplaintId",
                table: "ComplaintAnswers",
                column: "ComplaintId",
                principalTable: "Complaints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintAnswers_Users_WriterId",
                table: "ComplaintAnswers",
                column: "WriterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplaintAnswers_Complaints_ComplaintId",
                table: "ComplaintAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_ComplaintAnswers_Users_WriterId",
                table: "ComplaintAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComplaintAnswers",
                table: "ComplaintAnswers");

            migrationBuilder.RenameTable(
                name: "ComplaintAnswers",
                newName: "ComplaintAnswer");

            migrationBuilder.RenameIndex(
                name: "IX_ComplaintAnswers_WriterId",
                table: "ComplaintAnswer",
                newName: "IX_ComplaintAnswer_WriterId");

            migrationBuilder.RenameIndex(
                name: "IX_ComplaintAnswers_ComplaintId",
                table: "ComplaintAnswer",
                newName: "IX_ComplaintAnswer_ComplaintId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComplaintAnswer",
                table: "ComplaintAnswer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintAnswer_Complaints_ComplaintId",
                table: "ComplaintAnswer",
                column: "ComplaintId",
                principalTable: "Complaints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComplaintAnswer_Users_WriterId",
                table: "ComplaintAnswer",
                column: "WriterId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

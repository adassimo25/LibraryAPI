using Microsoft.EntityFrameworkCore.Migrations;

namespace LibraryAPI.Migrations
{
    public partial class SeedInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Books_CurrentStatusId",
                table: "Books",
                column: "CurrentStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_StatusHistory_CurrentStatusId",
                table: "Books",
                column: "CurrentStatusId",
                principalTable: "StatusHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_StatusHistory_CurrentStatusId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_CurrentStatusId",
                table: "Books");
        }
    }
}
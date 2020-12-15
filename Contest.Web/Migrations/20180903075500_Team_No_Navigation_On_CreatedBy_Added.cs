using Microsoft.EntityFrameworkCore.Migrations;

namespace Contest.Web.Migrations
{
    public partial class Team_No_Navigation_On_CreatedBy_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Participants_CreatedById",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_CreatedById",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Teams",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Teams");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Teams",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_CreatedById",
                table: "Teams",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Participants_CreatedById",
                table: "Teams",
                column: "CreatedById",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

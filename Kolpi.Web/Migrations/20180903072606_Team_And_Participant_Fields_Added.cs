using Microsoft.EntityFrameworkCore.Migrations;

namespace Kolpi.Web.Migrations
{
    public partial class Team_And_Participant_Fields_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RepoUrl",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Participants",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "RepoUrl",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Participants");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Kolpi.Web.Migrations
{
    public partial class Relation_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Teams_TeamId",
                table: "Participants");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "Participants",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Teams_TeamId",
                table: "Participants",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Teams_TeamId",
                table: "Participants");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "Participants",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Teams_TeamId",
                table: "Participants",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

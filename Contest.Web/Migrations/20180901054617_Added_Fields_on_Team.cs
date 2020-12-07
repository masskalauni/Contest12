using Microsoft.EntityFrameworkCore.Migrations;

namespace Contest.Web.Migrations
{
    public partial class Added_Fields_on_Team : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ITRequirements",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherRequirements",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTeamLead",
                table: "Participants",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ITRequirements",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "OtherRequirements",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "IsTeamLead",
                table: "Participants");
        }
    }
}

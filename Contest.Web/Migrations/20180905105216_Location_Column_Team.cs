using Microsoft.EntityFrameworkCore.Migrations;

namespace Contest.Web.Migrations
{
    public partial class Location_Column_Team : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Teams",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Teams");
        }
    }
}

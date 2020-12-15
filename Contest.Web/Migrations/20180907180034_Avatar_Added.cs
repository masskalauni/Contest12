using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Contest.Web.Migrations
{
    public partial class Avatar_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar",
                table: "Teams",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Teams");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kolpi.Web.Migrations
{
    public partial class Voted_On_Added_OnParticipantVotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "VotedOn",
                table: "ParticipantVotes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VotedOn",
                table: "ParticipantVotes");
        }
    }
}

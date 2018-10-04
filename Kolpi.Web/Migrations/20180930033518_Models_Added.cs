using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kolpi.Web.Migrations
{
    public partial class Models_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamScores");

            migrationBuilder.RenameColumn(
                name: "FinalScoreEarned",
                table: "Teams",
                newName: "AveragePeoplesChoiceScore");

            migrationBuilder.AddColumn<float>(
                name: "AverageImplementationScore",
                table: "Teams",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "AverageInnovationScore",
                table: "Teams",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "JudgeScores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InnovationScore = table.Column<float>(nullable: false),
                    UsefulnessScore = table.Column<float>(nullable: false),
                    QualityScore = table.Column<float>(nullable: false),
                    CompanyValueScore = table.Column<float>(nullable: false),
                    PresentationScore = table.Column<float>(nullable: false),
                    KolpiUserId = table.Column<string>(nullable: true),
                    TeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JudgeScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JudgeScores_Users_KolpiUserId",
                        column: x => x.KolpiUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JudgeScores_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantVotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserMail = table.Column<string>(nullable: true),
                    OrderOneTeam = table.Column<string>(nullable: true),
                    OrderTwoTeam = table.Column<string>(nullable: true),
                    OrderThreeTeam = table.Column<string>(nullable: true),
                    OrderFourTeam = table.Column<string>(nullable: true),
                    OrderFiveTeam = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantVotes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JudgeScores_KolpiUserId",
                table: "JudgeScores",
                column: "KolpiUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JudgeScores_TeamId",
                table: "JudgeScores",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JudgeScores");

            migrationBuilder.DropTable(
                name: "ParticipantVotes");

            migrationBuilder.DropColumn(
                name: "AverageImplementationScore",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "AverageInnovationScore",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "AveragePeoplesChoiceScore",
                table: "Teams",
                newName: "FinalScoreEarned");

            migrationBuilder.CreateTable(
                name: "TeamScores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyValueScore = table.Column<float>(nullable: false),
                    InnovationScore = table.Column<float>(nullable: false),
                    KolpiUserId = table.Column<string>(nullable: true),
                    PresentationScore = table.Column<float>(nullable: false),
                    QualityScore = table.Column<float>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    UsefulnessScore = table.Column<float>(nullable: false),
                    WeightedAverageScore = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamScores_Users_KolpiUserId",
                        column: x => x.KolpiUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamScores_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamScores_KolpiUserId",
                table: "TeamScores",
                column: "KolpiUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamScores_TeamId",
                table: "TeamScores",
                column: "TeamId");
        }
    }
}

using Microsoft.AspNetCore.Identity;

namespace Kolpi.Models.ScoreCard
{
    public class TeamScore
    {
        public TeamScore() { }
        public TeamScore(TeamScoreViewModel teamScoreViewModel)
        {
            Id = teamScoreViewModel.Id;
            InnovationScore = teamScoreViewModel.InnovationScore.Value;
            UsefulnessScore = teamScoreViewModel.UsefulnessScore.Value;
            QualityScore = teamScoreViewModel.QualityScore.Value;
            CompanyValueScore = teamScoreViewModel.CompanyValueScore.Value;
            PresentationScore = teamScoreViewModel.PresentationScore.Value;

            var teamCode = teamScoreViewModel.Team;
            InnovationAverageScore = teamScoreViewModel.WeightedAverageScore.Value;
            Team = Data.Teams.Find(teamCode);
        }

        public int Id { get; set; }
        public float InnovationScore { get; set; }
        public float UsefulnessScore { get; set; }
        public float QualityScore { get; set; }
        public float CompanyValueScore { get; set; }
        public float PresentationScore { get; set; }
        public float InnovationAverageScore { get; set; }
        public float ImplementationAverageScore { get; set; }

        public string KolpiUserId { get; set; }
        public IdentityUser KolpiUser { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}

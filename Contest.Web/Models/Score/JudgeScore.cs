using Microsoft.AspNetCore.Identity;

namespace Contest.Models.Score
{
    public class JudgeScore
    {
        public JudgeScore() { }
        public JudgeScore(JudgeScoreViewModel teamScoreViewModel)
        {
            Id = teamScoreViewModel.Id;
            InnovationScore = teamScoreViewModel.InnovationScore.Value;
            UsefulnessScore = teamScoreViewModel.UsefulnessScore.Value;
            QualityScore = teamScoreViewModel.QualityScore.Value;
            CompanyValueScore = teamScoreViewModel.CompanyValueScore.Value;
            PresentationScore = teamScoreViewModel.PresentationScore.Value;
        }

        public int Id { get; set; }
        public float InnovationScore { get; set; }
        public float UsefulnessScore { get; set; }
        public float QualityScore { get; set; }
        public float CompanyValueScore { get; set; }
        public float PresentationScore { get; set; }

        public string KolpiUserId { get; set; }
        public IdentityUser KolpiUser { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}

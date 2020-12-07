using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Kolpi.Enums;
using Kolpi.Web.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kolpi.Models.Score
{
    public class JudgeScoreViewModel
    {
        public int Id { get; set; }
        public List<SelectListItem> Teams { get; set; }

        [DisplayName("Team"), Required(ErrorMessage = "Team should be chosen to submit team score.")]
        public string Team { get; set; }

        public Theme Theme { get; set; }

        public string Participants { get; set; }

        [DisplayName("Idea and Team (Innovation, Multidiscplinary)")]
        [Required(ErrorMessage = "Score for 'Idea and Team' criteria is required")]
        [Range(ScoreRange.Min, ScoreRange.Max, ErrorMessage = "Score must be in range 1-10")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float? InnovationScore { get; set; }

        [DisplayName("Usefulness (Intuitiveness, Scalability)")]
        [Required(ErrorMessage = "Score for 'Usefulness' criteria is required")]
        [Range(ScoreRange.Min, ScoreRange.Max, ErrorMessage = "Score must be in range 1-10")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float? UsefulnessScore { get; set; }

        [DisplayName("Implementation (Functionality, Use of Technology)")]
        [Required(ErrorMessage = "Score for 'Quality' criteria is required")]
        [Range(ScoreRange.Min, ScoreRange.Max, ErrorMessage = "Score must be in range 1-10")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float? QualityScore { get; set; }

        [DisplayName("Experience (Value and Impact to Company)")]
        [Required(ErrorMessage = "Score for 'Experience' criteria is required")]
        [Range(ScoreRange.Min, ScoreRange.Max, ErrorMessage = "Score must be in range 1-10")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float? CompanyValueScore { get; set; }

        [DisplayName("Presentation & Demonstration")]
        [Required(ErrorMessage = "Score for 'Presentation' criteria is required")]
        [Range(ScoreRange.Min, ScoreRange.Max, ErrorMessage = "Score must be in range 1-10")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float? PresentationScore { get; set; }

        [DisplayName("Weighted Best Idea Score")]
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        public float? BestIdeaScore => InnovationScore * .5f 
            + CompanyValueScore * .4f 
            + PresentationScore * .1f;

        [DisplayName("Weighted Best Technical Implementation Score")]
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        public float?  BestTechnicalImplementationScore => QualityScore * .6f 
            + UsefulnessScore * .2f 
            + CompanyValueScore * .1f 
            + PresentationScore * .1f;

        [DisplayName("Weighted Average Score")]
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        public float? AverageScore => InnovationScore * .25f  
            + UsefulnessScore * .15f 
            + QualityScore * .30f 
            + CompanyValueScore * .15f 
            + PresentationScore * .15f;

        public string Judge { get; set; }

        public JudgeScoreViewModel() { }
        public JudgeScoreViewModel(JudgeScore judgeScore)
        {
            Id = judgeScore.Id;
            InnovationScore = judgeScore.InnovationScore;
            UsefulnessScore = judgeScore.UsefulnessScore;
            QualityScore = judgeScore.QualityScore;
            CompanyValueScore = judgeScore.CompanyValueScore;
            PresentationScore = judgeScore.PresentationScore;
            Team = judgeScore.Team.TeamName;
            Theme = judgeScore.Team.Theme;

            var userName = judgeScore.KolpiUser?.UserName;
            userName = userName?.Substring(0, userName.IndexOf('@'));
            userName = userName?.Replace('.', ' ');
            Judge = userName ?? "N/A";
        }
    }
}

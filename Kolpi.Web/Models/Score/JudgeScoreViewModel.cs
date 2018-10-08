using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Kolpi.Enums;
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

        [DisplayName("Innovation")]
        [Required(ErrorMessage = "Score for 'Innovation' criteria is required.")]
        [Range(1.0, 10.0, ErrorMessage = "Score should be in range 1.0-10.0")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float? InnovationScore { get; set; }

        [DisplayName("Usefulness (UI-UX / API)")]
        [Required(ErrorMessage = "Score for 'Usefulness' criteria is required.")]
        [Range(1.0, 10.0, ErrorMessage = "Score should be in range 1.0-10.0")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float? UsefulnessScore { get; set; }

        [DisplayName("Quality (Implementation)")]
        [Required(ErrorMessage = "Score for 'Quality' criteria is required.")]
        [Range(1.0, 10.0, ErrorMessage = "Score should be in range 1.0-10.0")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float? QualityScore { get; set; }

        [DisplayName("Value To Company")]
        [Required(ErrorMessage = "Score for 'Value To Company' criteria is required.")]
        [Range(1.0, 10.0, ErrorMessage = "Score should be in range 1.0-10.0")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float? CompanyValueScore { get; set; }

        [DisplayName("Presentation")]
        [Required(ErrorMessage = "Score for 'Presentation' criteria is required.")]
        [Range(1.0, 10.0, ErrorMessage = "Score should be in range 1.0-10.0")]
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
            Participants = string.Join(",", judgeScore.Team.Participants.ToList().Select(s => s.Name));

            var userName = judgeScore.KolpiUser?.UserName;
            userName = userName?.Substring(0, userName.IndexOf('@'));
            userName = userName?.Replace('.', ' ');
            Judge = userName ?? "N/A";
        }
    }
}

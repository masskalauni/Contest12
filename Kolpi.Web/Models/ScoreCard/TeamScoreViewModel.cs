using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Kolpi.Data;
using Kolpi.Enums;

namespace Kolpi.Models.ScoreCard
{
    public class TeamScoreViewModel
    {
        public int Id { get; set; }
        public List<SelectListItem> Teams { get; } = Data.Teams.All
            .Select(x => new SelectListItem { Value = x.TeamCode, Text = $"{x.TeamName} ( {x.Theme.ToString()} )" }).ToList();

        [DisplayName("Participating Team"), Required(ErrorMessage = "Team should be chosen to submit team score.")]
        public string Team { get; set; }

        [DisplayName("Innovation Score")]
        [Required(ErrorMessage = "Score for 'Innovation' criteria is required.")]
        [Range(1.0, 10.0, ErrorMessage = "Score rating should be in range 0.0-10.0")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float? InnovationScore { get; set; }

        [DisplayName("Usefulness Score")]
        [Required(ErrorMessage = "Score for 'Usefulness' criteria is required.")]
        [Range(1.0, 10.0, ErrorMessage = "Score rating should be in range 0.0-10.0")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float? UsefulnessScore { get; set; }

        [DisplayName("Quality Score")]
        [Required(ErrorMessage = "Score for 'Quality' criteria is required.")]
        [Range(1.0, 10.0, ErrorMessage = "Score rating should be in range 0.0-10.0")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float? QualityScore { get; set; }

        [DisplayName("Value To Company Score")]
        [Required(ErrorMessage = "Score for 'Value To Company' criteria is required.")]
        [Range(1.0, 10.0, ErrorMessage = "Score rating should be in range 0.0-10.0")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float? CompanyValueScore { get; set; }

        [DisplayName("Presentation Score")]
        [Required(ErrorMessage = "Score for 'Presentation' criteria is required.")]
        [Range(1.0, 10.0, ErrorMessage = "Score rating should be in range 0.0-10.0")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public float? PresentationScore { get; set; }

        [DisplayName("Weighted Final Average")]
        [DisplayFormat(DataFormatString = "{0:0.000}")]
        public float? WeightedAverageScore {
            get
            {
                var team = Data.Teams.Find(Team);

                if (team == null)
                {
                    return 0.0f;
                }

                return (WeightDictionary.ThemeWeights[team.Theme][JudgingCriteria.Innovation] * InnovationScore
                        + WeightDictionary.ThemeWeights[team.Theme][JudgingCriteria.UsefulNess] * UsefulnessScore
                        + WeightDictionary.ThemeWeights[team.Theme][JudgingCriteria.Quality] * QualityScore
                        + WeightDictionary.ThemeWeights[team.Theme][JudgingCriteria.ValueToCompany] * CompanyValueScore
                        + WeightDictionary.ThemeWeights[team.Theme][JudgingCriteria.Presentation] * PresentationScore) /
                       100;
            }
            set { }
        }

        public string Judge { get; set; }

        public TeamScoreViewModel() { }
        public TeamScoreViewModel(TeamScore teamScore)
        {
            Id = teamScore.Id;
            InnovationScore = teamScore.InnovationScore;
            UsefulnessScore = teamScore.UsefulnessScore;
            QualityScore = teamScore.QualityScore;
            CompanyValueScore = teamScore.CompanyValueScore;
            PresentationScore = teamScore.PresentationScore;
            WeightedAverageScore = teamScore.WeightedAverageScore;
            Team = teamScore.Team.TeamName;
            if (teamScore.KolpiUser == null) return;

            var userName = teamScore.KolpiUser.UserName;
            userName = userName?.Substring(0, userName.IndexOf('@'));
            userName = userName?.Replace('.', ' ');
            Judge = userName ?? "N/A";
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Kolpi.Enums;

namespace Kolpi.Models.ScoreCard
{
    public class TeamViewModel
    {
        public TeamViewModel()
        {
        }

        public TeamViewModel(Team team)
        {
            Id = team.Id;
            TeamCode = team.TeamCode;
            TeamName = team.TeamName;
            Theme = team.Theme;
            ProblemStatement = team.ProblemStatement;
            ITRequirements = team.ITRequirements;
            OtherRequirements = team.OtherRequirements;
            FinalScoreEarned = team.FinalScoreEarned;
            Participants = String.Join(Environment.NewLine, team.Participants
                ?.Select(x => $"{x.Name}, {x.Inumber}, {x.OfficeMail}, {x.Department}"));
            CreatedBy = $"{team.CreatedBy} [On {team.CreatedOn:MMM dd hh:mm tt}]";
        }
        public int Id { get; set; }
        public string TeamCode { get; set; }

        [Required(ErrorMessage = "Team name is required to add your team to event")]
        [Display(Name = "Team Name")]
        public string TeamName { get; set; }

        [Required, EnumDataType(typeof(Theme))]
        public Theme Theme { get; set; } = Theme.OpenIdea;

        [Required(ErrorMessage = "Provide your team's problem statement summary")]
        [DataType(DataType.MultilineText), Display(Name = "Problem Statement")]
        public string ProblemStatement { get; set; }

        [Display(Name = "IT Requirements")]
        public string ITRequirements { get; set; }

        [Display(Name = "Other Requirements")]
        public string OtherRequirements { get; set; }

        [Display(Name = "Team Created By")]
        public string CreatedBy { get; set; }

        [NotMapped]
        public float FinalScoreEarned { get; set; }

        [Required(ErrorMessage = "Enter your participants in provided format.")]
        [DataType(DataType.MultilineText), Display(Name = "Participants")]
        public string Participants { get; set; } = @"Bishnu Rawal, i82287, 9849182885, bishnu.rawal@verscend.com, R&D1\n\rNiraj Shah (Team Lead), i65001, 1111111111, niraj.shah@verscend.com, R&D1";
    }
}

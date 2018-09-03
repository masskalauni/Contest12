using System;
using System.Collections.Generic;
using Kolpi.Enums;
using System.Linq;
using System.Text.RegularExpressions;
using System.Security.Claims;

namespace Kolpi.Models.ScoreCard
{
    public class Team
    {
        public Team()
        {
        }

        public Team(TeamViewModel teamViewModel)
        {
            Id = teamViewModel.Id;
            TeamName = teamViewModel.TeamName;
            TeamCode = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
            Theme = teamViewModel.Theme;
            ProblemStatement = teamViewModel.ProblemStatement;
            ITRequirements = teamViewModel.ITRequirements;
            OtherRequirements = teamViewModel.OtherRequirements;            
            Participants = teamViewModel.Participants.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new Participant(x.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))).ToList();
            CreatedOn = DateTime.Now;
            CreatedBy = teamViewModel.CreatedBy;
        }

        public int Id { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public Theme Theme { get; set; }
        public string ProblemStatement { get; set; }
        public string RepoUrl { get; set; }        
        public float FinalScoreEarned { get; set; }
        public string ITRequirements { get; set; }
        public string OtherRequirements { get; set; }        
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
    }
}
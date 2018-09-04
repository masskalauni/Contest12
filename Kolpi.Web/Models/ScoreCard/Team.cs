﻿using System;
using System.Collections.Generic;
using Kolpi.Enums;
using System.Linq;
using System.Text.RegularExpressions;

namespace Kolpi.Models.ScoreCard
{
    public class Team
    {
        public Team()
        {
        }

        public Team(TeamViewModel teamViewModel, IList<Participant> participants)
        {
            Id = teamViewModel.Id;
            TeamCode = teamViewModel.TeamCode;
            TeamName = teamViewModel.TeamName;            
            Theme = teamViewModel.Theme;
            ProblemStatement = teamViewModel.ProblemStatement;
            ITRequirements = teamViewModel.ITRequirements;
            OtherRequirements = teamViewModel.OtherRequirements;
            Participants = participants;
            CreatedOn = teamViewModel.CreatedOn;
            CreatedBy = teamViewModel.CreatedBy;
            RepoUrl = teamViewModel.RepoUrl;
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
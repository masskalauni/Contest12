using System;
using System.Collections.Generic;
using Kolpi.Enums;
using System.IO;

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
            Location = teamViewModel.Location;

            //Convert uploaded image to byte array if there is one
            if (teamViewModel?.Avatar?.Length > 0)
            {
                byte[] avatarBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    teamViewModel.Avatar.CopyTo(memoryStream);
                    avatarBytes = memoryStream.ToArray();
                }
                Avatar = avatarBytes;
            }
        }

        public int Id { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public byte[] Avatar { get; set; }
        public Theme Theme { get; set; }
        public string ProblemStatement { get; set; }
        public string RepoUrl { get; set; }        
        public float InnovationScore { get; set; }
        public float ImplementationScore { get; set; }
        public float PeoplesChoiceScore { get; set; }
        public string ITRequirements { get; set; }
        public string OtherRequirements { get; set; }
        public string Location { get; set; } = "Kathmandu";
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }
    }
}
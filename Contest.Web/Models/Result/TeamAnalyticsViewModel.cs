using System.Collections.Generic;

namespace Contest.Models.Score
{
    public class TeamAnalyticsViewModel
    {
        public int TotalTeams { get; set; }
        public int TotalParticipants { get; set; }
        public List<(string Name, string Email, string Team, string Department)> AllParticipants { get; set; }
        public List<(string TeamName, string theme, int ParticipantCount)> AllTeams { get; set; }
        public List<(string Team, string IT, string Other)> TeamRequirements { get; set; }
    }
}

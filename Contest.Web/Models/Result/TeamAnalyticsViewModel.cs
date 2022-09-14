using Contest.Enums;
using Contest.Web.Constants;
using System.Collections.Generic;

namespace Contest.Models.Score
{
    public class TeamAnalyticsViewModel
    {
        public int TotalTeams { get; set; }
        public int TotalParticipants { get; set; }
        public List<(Theme Theme, int TeamCount, List<string> TeamList)> TeamsByTheme { get; set; }
        public int ThemesSelectedCount { get; set; }
        public List<(string Location, int TeamCount, List<string> TeamList)> TeamsByLocation { get; set; }
        public List<string> AllParticipants { get; set; }
        public List<string> AllTeams { get; set; }
        public List<(string Team, string IT, string Other)> TeamRequirements { get; set; }
    }
}

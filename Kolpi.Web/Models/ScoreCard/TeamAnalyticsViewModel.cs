using Kolpi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kolpi.Models.ScoreCard
{
    public class TeamAnalyticsViewModel
    {
        public int TotalTeams { get; set; }
        public int TotalParticipants { get; set; }
        public Dictionary<Theme, int> ThemeSelection { get; set; }
        
    }
}

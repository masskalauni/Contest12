using System.Collections.Generic;
using System.Linq;
using Kolpi.Models.ScoreCard;
using Kolpi.Enums;

namespace Kolpi.Data
{
    public class Teams
    {
        public static IList<Team> All =>
            new List<Team>
            {
                new Team {TeamName = "Team Deft Plus", Theme = Theme.OpenIdea},
                new Team {TeamName = "Team MS4", Theme = Theme.ArtificialIntelligence},
                new Team {TeamName = "Team él 7th Sense", Theme = Theme.ArtificialIntelligence},
                new Team {TeamName = "Team Five", Theme = Theme.DataVisualizationAndReporting},
                new Team {TeamName = "The Qavengers", Theme = Theme.OpenIdea},
                new Team {TeamName = "Gold Miners", Theme = Theme.DataVisualizationAndReporting},
                new Team {TeamName = "Crumblers", Theme = Theme.DataVisualizationAndReporting},
                new Team {TeamName = "Team STWD", Theme = Theme.DataVisualizationAndReporting},
                new Team {TeamName = "Team OCP", Theme = Theme.OpenIdea},
                new Team {TeamName = "Troll", Theme = Theme.DataVisualizationAndReporting},
                new Team {TeamName = "Deus Ex Machina", Theme = Theme.OpenIdea},
                new Team {TeamName = "Team DEFT", Theme = Theme.DataVisualizationAndReporting},
                new Team {TeamName = "Vizards", Theme = Theme.OpenIdea},
                new Team {TeamName = "Makura", Theme = Theme.Security},
                new Team {TeamName = "Team Dragons", Theme = Theme.Security}
            };
        public static Team Find(string teamCodeOrName) => All.FirstOrDefault(x => x.TeamCode == teamCodeOrName || x.TeamName == teamCodeOrName);
    }
}

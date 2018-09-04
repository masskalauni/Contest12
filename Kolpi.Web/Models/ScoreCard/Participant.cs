using System;

namespace Kolpi.Models.ScoreCard
{
    public class Participant
    {
        public Participant()
        {
        }

        public Participant(string[] propValues)
        {
            Name = propValues[0].Trim();
            IsTeamLead = !string.IsNullOrWhiteSpace(Name) && Name.IndexOf("lead", StringComparison.InvariantCultureIgnoreCase) != -1;
            Inumber = propValues[1].Trim();
            Phone = propValues[2].Trim();
            OfficeMail = propValues[3].Trim();
            Department = propValues[4].Trim();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string OfficeMail { get; set; }
        public string Inumber { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public bool IsTeamLead { get; set; } = false;
        public Team Team { get; set; }
    }
}
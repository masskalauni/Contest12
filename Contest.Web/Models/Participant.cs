using System;

namespace Contest.Models.Score
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
            OfficeMail = propValues[1].Trim();
            Department = propValues[2].Trim();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string OfficeMail { get; set; }
        public string Inumber { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public bool IsTeamLead { get; set; } = false;
        
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
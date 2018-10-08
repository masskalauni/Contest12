using Kolpi.Models.Score;
using System.Collections.Generic;

namespace Kolpi.Models.Result
{
    public class FinalResultViewModel
    {
        public IList<TeamViewModel> TeamsScores { get; set; }
        public IList<JudgeScoreViewModel> JudgesScores  { get; set; }
        public (IList<(string Value, string Text, int FinalScore)> Teams, IList<ParticipantVoteViewModel> AllVotes) PeoplesChoiceRanks { get; set; }
    }
}

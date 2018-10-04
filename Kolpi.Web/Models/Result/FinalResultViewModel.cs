using Kolpi.Models.Score;
using System.Collections.Generic;

namespace Kolpi.Models.Result
{
    public class FinalResultViewModel
    {
        public IList<TeamViewModel> BestIdeaTeams { get; set; }
        public IList<TeamViewModel> BestImplementationTeams { get; set; }
        public IList<TeamViewModel> PeoplesChoiceTeams { get; set; }
        public IList<JudgeScoreViewModel> JudgesScores  { get; set; }
    }
}

using System.Collections.Generic;

namespace Kolpi.Models.ScoreCard
{
    public class ResultViewModel
    {
        public IList<TeamViewModel> Teams { get; set; }
        public IList<TeamScoreViewModel> TeamScores  { get; set; }
    }
}

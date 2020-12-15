using System;
using System.Collections.Generic;
using System.Linq;
using Contest.Data;

namespace Contest.Models.Survey
{ 
    public class SurveyResultViewModel
    {
        public int TotalResponses { get; set; }
        public double AverageRating { get; set; }
        public IList<string> Goods { get; set; }
        public IList<string> Oks { get; set; }
        public IList<string> Bads { get; set; }
        public int InterestedInFutureTotal { get; set; }
        public IList<FixedFeatureSelectionsStat> FixedFeaturesSelections { get; set; }

        public IList<string> FutureThemeList { get; set; }
        public IList<string> ExpectedFeatureList { get; set; }
        public IList<string> JudigProcessImprovementList { get; set; }

        public IList<FeedbackViewModel> FeedbackViewModels { get; set; }

        public SurveyResultViewModel() { }

        public SurveyResultViewModel(IEnumerable<Feedback> feedbacks)
        {
            var feedBackList = feedbacks as IList<Feedback> ?? feedbacks.ToList();
            FeedbackViewModels = feedBackList.Select(x => new FeedbackViewModel(x)).ToList();
            TotalResponses = feedBackList.Count;
            AverageRating = Math.Round(feedBackList.Average(x => x.OverallRating), 2);
            Goods = feedBackList.Where(x => !string.IsNullOrWhiteSpace(x.Good)).Select(x => x.Good.Trim()).ToList();
            Oks = feedBackList.Where(x => !string.IsNullOrWhiteSpace(x.Ok)).Select(x => x.Ok.Trim()).ToList();
            Bads = feedBackList.Where(x => !string.IsNullOrWhiteSpace(x.Bad)).Select(x => x.Bad.Trim()).ToList();
            InterestedInFutureTotal = feedBackList.Count(x => x.InterestedInFuture);

            FixedFeaturesSelections = feedBackList.SelectMany(feedback =>
                EventFeatures.All.Where(x => feedback.SelectedFeatures.Split(',').Contains(x.FeatureId.ToString()))
                    .Select(x => x)).GroupBy(z => z.Name).Select(y => new FixedFeatureSelectionsStat { FeatureName = y.Key, SelectionCount = y.Count()}).ToList();

            FutureThemeList = feedBackList.Where(x => !string.IsNullOrWhiteSpace(x.FutureTheme)).Select(x => x.FutureTheme.Trim()).ToList();
            ExpectedFeatureList = feedBackList.Where(x => !string.IsNullOrWhiteSpace(x.ExpectedFeatures)).Select(x => x.ExpectedFeatures.Trim()).ToList();
            JudigProcessImprovementList = feedBackList.Where(x => !string.IsNullOrWhiteSpace(x.JudgingProcessImprovements)).Select(x => x.JudgingProcessImprovements.Trim()).ToList();
        }
    }
}
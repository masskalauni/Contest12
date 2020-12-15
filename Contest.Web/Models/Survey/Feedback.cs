using System.Linq;

namespace Contest.Models.Survey
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte OverallRating { get; set; }
        public string Good { get; set; }
        public string Ok { get; set; }
        public string Bad { get; set; }
        public bool InterestedInFuture { get; set; }
        public string FutureTheme { get; set; }
        public string ExpectedFeatures { get; set; }
        public string SelectedFeatures { get; set; }
        public string JudgingProcessImprovements { get; set; }

        public int SurveyThreadId { get; set; }
        public SurveyThread SurveyThread { get; set; }

        public Feedback() { }

        public Feedback(FeedbackViewModel viewModel)
        {
            Name = viewModel.Name;
            OverallRating = viewModel.OverallRating;
            Good = viewModel.Good;
            Ok = viewModel.Ok;
            Bad = viewModel.Bad;
            InterestedInFuture = viewModel.InterestedInFuture;
            FutureTheme = viewModel.FutureTheme;
            ExpectedFeatures = viewModel.ExpectedFeatures;
            var selectedFeatures = viewModel.EventFeatureViewModels.Where(x => x.Selected).ToList();
            SelectedFeatures = selectedFeatures.Any() ? string.Join(",", selectedFeatures.Select(y => y.FeatureId.ToString())) : "";
            JudgingProcessImprovements = viewModel.JudgingProcessImprovements;
            SurveyThreadId = viewModel.SurveyThreadId;
        }
    }
}

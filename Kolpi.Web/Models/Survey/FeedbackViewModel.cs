using Kolpi.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Kolpi.Models.Survey
{
    [NotMapped]
    public class FeedbackViewModel
    {
        public FeedbackViewModel() { }

        public FeedbackViewModel(Feedback feedback)
        {
            Id = feedback.Id;
            SurveyThreadId = feedback.SurveyThread.Id;
            Name = feedback.Name;
            ThreadHash = feedback.SurveyThread.ThreadHash;
            OverallRating = feedback.OverallRating;
            Good = feedback.Good;
            Ok = feedback.Ok;
            Bad = feedback.Bad;
            InterestedInFuture = feedback.InterestedInFuture;
            FutureTheme = feedback.FutureTheme;
            ExpectedFeatures = feedback.ExpectedFeatures;
            JudgingProcessImprovements = feedback.JudgingProcessImprovements;
            EventFeatureViewModels = EventFeatures.All
                .Where(x => feedback.SelectedFeatures.Split(',').Contains(x.FeatureId.ToString())).Select(x => x).ToList();
        }

        public int Id { get; set; }
        public int SurveyThreadId { get; set; }
        public string ThreadHash { get; set; }

        [DisplayName("Your name")]
        public string Name { get; set; }

        [DisplayName("Your love for overall event")]
        [Required(ErrorMessage = "Please rate the event")]
        public byte OverallRating { get; set; }

        [DisplayName("Good/Best things about Event")]
        [MaxLength(1000)]
        [MinLength(10, ErrorMessage = "Please drop at least few words/sentences (10 chars minimum), Your feedback is important.")]
        public string Good { get; set; }

        [DisplayName("Ok things about Event")]
        [MaxLength(1000)]
        public string Ok { get; set; }

        [DisplayName("Bad things about Event")]
        [MaxLength(1000)]
        public string Bad { get; set; }
        [DisplayName("I will be part of this event in future")]
        public bool InterestedInFuture { get; set; }
        [DisplayName("And I have theme ideas too")]
        public string FutureTheme { get; set; }

        [DisplayName("Event features and improvements")]
        public IList<EventFeatureViewModel> EventFeatureViewModels { get; set; } = EventFeatures.All;

        [DisplayName("Any other needed features?")]
        public string ExpectedFeatures { get; set; }

        public string JudgingProcessImprovements { get; set; }
    }
}

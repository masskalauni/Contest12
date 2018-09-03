using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kolpi.Models.Survey
{
    public class SurveyThreadViewModel
    {
        public SurveyThreadViewModel() { }
        public SurveyThreadViewModel(SurveyThread surveyThread)
        {
            UserName = surveyThread.KolpiUser.UserName;
            Description = surveyThread.Description;
            AbsoluteUrl = surveyThread.AbsoluteUrl;
            ThreadHash = surveyThread.ThreadHash;
            FeedbackCount = surveyThread.Feedbacks.Count;
        }

        public string UserId { get; set; }
        public string ThreadHash { get; set; }
        public string UserName { get; set; }
        public int FeedbackCount { get; set; }

        [Required, DisplayName("Survey Title")]
        public string Description { get; set; }

        [DisplayName("Url to Share Publicly")]
        public string AbsoluteUrl { get; set; }
    }
}

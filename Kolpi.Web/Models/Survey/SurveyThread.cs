using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Kolpi.Models.Survey
{
    public class SurveyThread
    {
        public SurveyThread() { }
        public SurveyThread(SurveyThreadViewModel surveyViewModel)
        {
            Description = surveyViewModel.Description;
            AbsoluteUrl = surveyViewModel.AbsoluteUrl;
            ThreadHash = surveyViewModel.ThreadHash;
            KolpiUserId = surveyViewModel.UserId;
        }

        public int Id { get; set; }
        public string ThreadHash { get; set; }
        public string Description  { get; set; }
        public string AbsoluteUrl { get; set; }

        public ICollection<Feedback> Feedbacks { get; set; }

        public string KolpiUserId { get; set; }
        public IdentityUser KolpiUser { get; set; }

    }
}

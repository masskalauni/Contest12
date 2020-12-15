using System.Collections.Generic;
using Contest.Models.Survey;

namespace Contest.Data
{
    public class EventFeatures
    {
        public static IList<EventFeatureViewModel> All =>
            new List<EventFeatureViewModel>()
            {
                new EventFeatureViewModel {FeatureId = 1, Name = "Enable more than one solutions to be submitted.", Selected = false},
                new EventFeatureViewModel {FeatureId = 2, Name = "Ability to see performacne of your model against larger and real data set.", Selected = false},
                new EventFeatureViewModel {FeatureId = 3, Name = "Judging process could have been better?", Selected = false},
                new EventFeatureViewModel {FeatureId = 4, Name = "Mentoring for teams need to be included.", Selected = false}
            };
    }
}

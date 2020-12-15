using System.ComponentModel.DataAnnotations;

namespace Contest.Models.Survey
{
    public class EventFeature
    {
        [Key]
        public int FeatureId { get; set; }
        public string Name { get; set; }
    }
}
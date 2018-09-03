using System.ComponentModel.DataAnnotations;

namespace Kolpi.Models.Survey
{
    public class EventFeature
    {
        [Key]
        public int FeatureId { get; set; }
        public string Name { get; set; }
    }
}
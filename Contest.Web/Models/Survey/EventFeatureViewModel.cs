using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contest.Models.Survey
{
    [NotMapped]
    public class EventFeatureViewModel
    {
        [Key]
        public int FeatureId { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}

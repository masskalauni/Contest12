using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kolpi.Models.Score
{
    public class ParticipantVoteViewModel
    {
        public ParticipantVoteViewModel()
        {
        }

        public ParticipantVoteViewModel(ParticipantVote participantVote)
        {
            Id = participantVote.Id;
            UserId = participantVote.UserId;
            OrderOneTeam = participantVote.OrderOneTeam;
            OrderTwoTeam = participantVote.OrderTwoTeam;
            OrderThreeTeam = participantVote.OrderThreeTeam;
            OrderFourTeam = participantVote.OrderFourTeam;
            OrderFiveTeam = participantVote.OrderFiveTeam;
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IList<SelectListItem> Teams { get; set; }

        [Required(ErrorMessage = "Who is the top team in your opninion?")]
        public string OrderOneTeam { get; set; }

        [Required(ErrorMessage = "Who is the second team in your opninion?")]
        public string OrderTwoTeam { get; set; }

        [Required(ErrorMessage = "Who is the third team in your opninion?")]
        public string OrderThreeTeam { get; set; }

        [Required(ErrorMessage = "Who is the fourth team in your opninion?")]
        public string OrderFourTeam { get; set; }

        [Required(ErrorMessage = "Who is the fifth team in your opninion?")]
        public string OrderFiveTeam { get; set; }
    }
}

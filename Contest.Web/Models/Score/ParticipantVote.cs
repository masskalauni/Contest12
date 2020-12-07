using System;

namespace Contest.Models.Score
{
    public class ParticipantVote
    {
        public ParticipantVote()
        {
        }

        public ParticipantVote(ParticipantVoteViewModel participantVoteViewModel)
        {
            Id = participantVoteViewModel.Id;
            UserId = participantVoteViewModel.UserId;
            OrderOneTeam = participantVoteViewModel.OrderOneTeam;
            OrderTwoTeam = participantVoteViewModel.OrderTwoTeam;
            OrderThreeTeam = participantVoteViewModel.OrderThreeTeam;
            OrderFourTeam = participantVoteViewModel.OrderFourTeam;
            OrderFiveTeam = participantVoteViewModel.OrderFiveTeam;
            VotedOn = participantVoteViewModel.VotedOn;
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string OrderOneTeam { get; set; }
        public string OrderTwoTeam { get; set; }
        public string OrderThreeTeam { get; set; }
        public string OrderFourTeam { get; set; }
        public string OrderFiveTeam { get; set; }
        public DateTime? VotedOn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Kolpi.Data;
using Kolpi.Models.Score;
using Kolpi.Web.Common;
using Kolpi.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kolpi.Web.Controllers
{
    [Authorize]
    public class PeoplesChoiceController : Controller
    {
        private readonly KolpiDbContext _context;
        private readonly IConfiguration _configuration;

        public PeoplesChoiceController(KolpiDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> VoteTeams()
        {
            var participantVoteViewModel = new ParticipantVoteViewModel { Teams = await GetTeamsSelectList() };
            return View(participantVoteViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AllVotes()
        {
            var allVotes = await _context.ParticipantVotes.Where(x => x.VotedOn.IsCurrentYear()).ToListAsync();            
            var allVoteViewModels = allVotes.Select(x => new ParticipantVoteViewModel(x)
            {
                UserName = "Hidden"
            }).ToList();

            IList<(string Code, string Detail)> allTeams = await GetTeamsFormatted();
            IList<(string Code, string Detail, int FinalScore)> allTeamsScoreAdded = new List<(string, string, int)>();

            int finalScoreSum;

            foreach (var (Code, Detail) in allTeams)
            {
                finalScoreSum = 0;
                foreach (var vote in allVoteViewModels)
                {
                    if (Code.Equals(vote.OrderOneTeam))
                    {
                        finalScoreSum += Score.RankOne;
                    }
                    else if (Code.Equals(vote.OrderTwoTeam))
                    {
                        finalScoreSum += Score.RankTwo;
                    }
                    else if (Code.Equals(vote.OrderThreeTeam))
                    {
                        finalScoreSum += Score.RankThree;
                    }
                    else if (Code.Equals(vote.OrderFourTeam))
                    {
                        finalScoreSum += Score.RankFour;
                    }
                    else if (Code.Equals(vote.OrderFiveTeam))
                    {
                        finalScoreSum += Score.RankFive;
                    }
                }
                allTeamsScoreAdded.Add((Code, Detail, finalScoreSum));
            }

            (IList<(string Value, string Text, int FinalScore)> Teams, IList<ParticipantVoteViewModel> AllVoteViewModels) returnData = (allTeamsScoreAdded, allVoteViewModels);

            return View(returnData);
        }

        [HttpPost]
        public async Task<IActionResult> VoteTeams(ParticipantVoteViewModel voteViewModel)
        {
            var editSetting = await _context.Settings.FirstOrDefaultAsync(x => x.Name == Setting.Voting);
            var allowVoting = editSetting?.Value == "1";

            if (!allowVoting)
            {
                ModelState.AddModelError("", $"Voting Disabled: Bear with us! Once judeges submit their scores, we will open this up to vote for teams of your choice.");
                voteViewModel.Teams = await GetTeamsSelectList();
                return View(voteViewModel);
            }

            if (ModelState.IsValid)
            {
                // Validation - dropdown uniqueness
                var votes = new List<string> {
                    voteViewModel.OrderOneTeam,
                    voteViewModel.OrderTwoTeam,
                    voteViewModel.OrderThreeTeam,
                    voteViewModel.OrderFourTeam,
                    voteViewModel.OrderFiveTeam };

                if (votes.Distinct().Count() != votes.Count)
                {
                    ModelState.AddModelError("", $"You can't make same team take multiple ranks, or do you? Your top five selections must be unique.");
                    voteViewModel.Teams = await GetTeamsSelectList();
                    return View(voteViewModel);
                }

                var me = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                //Validation - Has user already voted?
                var record = await _context.ParticipantVotes.Where(x => x.UserId.Equals(me) && x.VotedOn.IsCurrentYear()).ToListAsync();

                if (record.Any())
                    return RedirectToAction(nameof(HomeController.Error), "Home", new { errorCode = "Already Voted", message = $"Don't be so greedy :), You have aleady voted for people's choice award. Thanks!" });

                var participantVote = new ParticipantVote(voteViewModel)
                {
                    UserId = me,
                    VotedOn = DateTime.Now
                };

                _context.Add(participantVote);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(PeoplesChoiceController.Thanks));
            }

            //Something went wrong, keep user on same page
            return View(voteViewModel);
        }

        [HttpGet]
        public ViewResult Thanks()
        {
            return View();
        }

        private async Task<IList<SelectListItem>> GetTeamsSelectList()
        {
            IList<(string Value, string Text)> teamList = await GetTeamsFormatted();
            return teamList.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
        }

        private async Task<IList<(string Value, string Text)>> GetTeamsFormatted()
        {
            var teams = await _context.Teams.Where(x => x.CreatedOn.IsCurrentYear()).Include(x => x.Participants).ToListAsync();
            IList<(string Value, string Text)> teamList = teams.Select(t => (t.TeamCode,
                $"{t.TeamName} ({t.Theme.ToString()} - {string.Join(", ", t.Participants.Select(x => x.Name.Split()[0]))} )")).ToList();
            return teamList;
        }
    }
}
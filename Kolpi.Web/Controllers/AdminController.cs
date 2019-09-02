using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kolpi.Data;
using Kolpi.Models.Result;
using Kolpi.Models.Score;
using Kolpi.Web.Constants;
using Kolpi.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Kolpi.Web.Common;


namespace Kolpi.Web.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class AdminController : Controller
    {
        private readonly KolpiDbContext _context;
        private readonly IConfiguration _configuration;

        public AdminController(KolpiDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var editSetting = await _context.Settings.FirstOrDefaultAsync(x => x.Name == Setting.FinalResult);
            var allowFinalResult = editSetting?.Value == "1";

            if (!allowFinalResult)
                return View("Error", new ErrorViewModel { ErrorCode = "Final Result Disabled", Message = "Final result not disclosed yet to avoid bias and judgement conflicts." });

            List<JudgeScore> judgeScores = await _context.JudgeScores.Where(x => x.Team.CreatedOn.IsCurrentYear()).Include(x => x.Team.Participants).Include(u => u.KolpiUser).ToListAsync();
            List<JudgeScoreViewModel> judgeScoresViewModels = judgeScores.Select(x => new JudgeScoreViewModel(x)
            {
                Participants = string.Join(",", x.Team.Participants.ToList().Select(s => s.Name))
            }).ToList();
            List<ParticipantVote> allVotes = await _context.ParticipantVotes.Where(x => x.VotedOn.IsCurrentYear()).ToListAsync();
            List<IdentityUser> allUsers = await _context.Users.ToListAsync();


            //The weighted average (x) is equal to the sum of the product of the weight (wi) times the data number (xi) divided by the sum of the weights
            List<TeamViewModel> teamsFinalScores = judgeScoresViewModels.GroupBy(x => new { x.Team, x.Theme, x.Participants })
                .Select(g => new TeamViewModel
                {
                    TeamName = g.Key.Team,
                    AveragePlainAverage = g.Average(x => x.AverageScore.Value),
                    AverageBestIdeaScore = g.Average(x => x.BestIdeaScore.Value),
                    AverageBestImplementationScore = g.Average(x => x.BestTechnicalImplementationScore.Value),
                    Theme = g.Key.Theme,
                    Participants = g.Key.Participants
                }).ToList();


            var allVoteViewModels = allVotes.Select(x => new ParticipantVoteViewModel(x)
            {
                UserName = allUsers.FirstOrDefault(y => y.Id == x.UserId)?.UserName
            }).OrderBy(x => x.UserName).ToList();

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

            var peopleChoices = (allTeamsScoreAdded, allVoteViewModels);

            var finalResult = new FinalResultViewModel
            {
                TeamsScores = teamsFinalScores,
                JudgesScores = judgeScoresViewModels,
                PeoplesChoiceRanks = peopleChoices
            };

            return View(finalResult);
        }

        private async Task<IList<(string Value, string Text)>> GetTeamsFormatted()
        {
            var teams = await _context.Teams.Where(x => x.CreatedOn.IsCurrentYear()).Include(x => x.Participants).ToListAsync();
            IList<(string Value, string Text)> teamList = teams.Select(t => (t.TeamCode,
                $"{t.TeamName} ({t.Theme.ToString()} - {string.Join(", ", t.Participants.Select(x => x.Name.Split()[0]))} )")).ToList();
            return teamList;
        }

        public IActionResult Error(string errorCode, string message)
        {
            return View(new ErrorViewModel { ErrorCode = errorCode, Message = message });
        }
    }
}

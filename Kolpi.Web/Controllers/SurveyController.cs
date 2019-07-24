using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kolpi.Data;
using Kolpi.Models.Survey;
using Kolpi.Models.Score;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Kolpi.Web.Constants;
using Microsoft.Extensions.Configuration;

namespace Kolpi.Web.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        private readonly KolpiDbContext _context;
        private readonly IConfiguration _configuration;

        public SurveyController(KolpiDbContext context, IConfiguration configuration)
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
        public async Task<IActionResult> PeoplesChoiceVotes()
        {
            var allVotes = await _context.ParticipantVotes.ToListAsync();
            var allUsers = await _context.Users.ToListAsync();

            var allVoteViewModels = allVotes.Select(x => new ParticipantVoteViewModel(x)
            {
                UserName = allUsers.FirstOrDefault(y => y.Id == x.UserId)?.UserName
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
            if (!_configuration.GetSection("AppSettings:AllowVoting").Get<bool>())
            {
                ModelState.AddModelError("", $"Polling timespan already expired, you can't vote now.");
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
                    ModelState.AddModelError("", $"You can't make same team take multiple ranks, or do you? Team selection on individual dropdown must be unique.");
                    voteViewModel.Teams = await GetTeamsSelectList();
                    return View(voteViewModel);
                }

                var me = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                //Validation - Has user already voted?
                var record = await _context.ParticipantVotes.Where(x => x.UserId.Equals(me)).ToListAsync();

                if (record.Any())
                    return RedirectToAction(nameof(HomeController.Error), "Home", new { errorCode = "Already Voted", message = $"Don't be so greedy :), You have aleady voted for people's choice award. Thanks!" });

                var participantVote = new ParticipantVote(voteViewModel)
                {
                    UserId = me
                };

                _context.Add(participantVote);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            //Something went wrong, keep user on same page
            return View(voteViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AdminIndex()
        {
            var allSurveys = await _context.SurveyThreads.Include(x => x.Feedbacks).Include(y => y.KolpiUser).ToListAsync();
            var allSurveyViewModels = allSurveys.Select(x => new SurveyThreadViewModel(x));
            return View(allSurveyViewModels);
        }

        [HttpGet]
        public IActionResult CreateSurveyThread()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSurveyThread(SurveyThreadViewModel surveyViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(surveyViewModel);
            }

            var buffer = new byte[5];
            new Random().NextBytes(buffer);
            var hash = string.Join("", buffer.Select(b => b.ToString("X2")));

            surveyViewModel.ThreadHash = hash;
            surveyViewModel.AbsoluteUrl = $"{Request.Scheme}://{Request.Host.Value}/feedback/{hash}";
            surveyViewModel.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var survey = new SurveyThread(surveyViewModel);

            _context.Add(survey);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AdminIndex));
        }

        [HttpGet]
        public async Task<IActionResult> ThreadFeedbacks(string threadHash)
        {
            if (string.IsNullOrWhiteSpace(threadHash))
                return NotFound("ThreadHash not supplied");
            var feedbacks = await _context.Feedbacks.Include(y => y.SurveyThread).Where(x => x.SurveyThread.ThreadHash == threadHash).ToListAsync();


            var resultViewModel = new SurveyResultViewModel(feedbacks);
            return View(resultViewModel);
        }

        [AllowAnonymous]
        [Route("feedback/{threadHash}")]
        public IActionResult SubmitFeedback(string threadHash)
        {
            var thread = _context.SurveyThreads.FirstOrDefault(x => x.ThreadHash == threadHash);
            ViewData["Description"] = thread.Description;
            return View(new FeedbackViewModel { SurveyThreadId = thread.Id });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("feedback/{threadHash}")]
        public async Task<IActionResult> SubmitFeedback(string threadHash, FeedbackViewModel surveyViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var survey = new Feedback(surveyViewModel);
            _context.Add(survey);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Thanks));
        }

        [AllowAnonymous]
        public IActionResult Thanks()
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
            var teams = await _context.Teams.Include(x => x.Participants).ToListAsync();
            IList<(string Value, string Text)> teamList = teams.Select(t => (t.TeamCode,
                $"{t.TeamName} ({t.Theme.ToString()} - {string.Join(", ", t.Participants.Select(x => x.Name.Split()[0]))} )")).ToList();
            return teamList;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kolpi.Data;
using Kolpi.Models.Result;
using Kolpi.Models.Score;
using Kolpi.Web.Constants;
using Kolpi.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kolpi.Web.Controllers
{
    [Authorize(Roles = Role.Admin)]
    public class AdminController : Controller
    {
        private readonly KolpiDbContext _context;
        public AdminController(KolpiDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //if (DateTime.Now < DateTime.Parse("2017-11-30 11:00 AM"))
            //{
            //    return RedirectToAction("Error", new { errorCode = "Feature Access Time", message = "This feature is will be available 2017-11-30 4:00 PM. Respect your curiosity, THANKS!" });
            //}

            var judgeScores = await _context.JudgeScores.Include(t => t.Team).Include(u => u.KolpiUser).ToListAsync();
            var judgeScoresViewModels = judgeScores.Select(x => new JudgeScoreViewModel(x)).ToList();


            //The weighted average (x) is equal to the sum of the product of the weight (wi) times the data number (xi) divided by the sum of the weights
            var teamsFinalScores = judgeScoresViewModels.GroupBy(x => x.Team)
                .Select((g, h) => new TeamViewModel
                {
                    TeamName = g.Key,
                    AverageBestIdeaScore = g.Sum(x => x.BestIdeaScore.Value) / g.Count(),
                    AverageBestImplementationScore = g.Sum(x => x.BestTechnicalImplementationScore.Value) / g.Count(),
                    //Theme = g.
                }).ToList();

            var bestIdeaTeams = teamsFinalScores.OrderByDescending(x => x.AverageBestIdeaScore).ToList();
            var bestImplementationTeams = teamsFinalScores.OrderByDescending(x => x.AverageBestImplementationScore).ToList();
            var peoplesChoiceTeams = teamsFinalScores.OrderByDescending(x => x.AveragePeoplesChoiceScore).ToList();

            var finalResult = new FinalResultViewModel
            {
                BestIdeaTeams = bestIdeaTeams,
                BestImplementationTeams = bestImplementationTeams,
                PeoplesChoiceTeams = peoplesChoiceTeams,
                JudgesScores = new List<JudgeScoreViewModel>()
            };

            return View(finalResult);
        }

        public IActionResult Error(string errorCode, string message)
        {
            return View(new ErrorViewModel { ErrorCode = errorCode, Message = message });
        }
    }
}

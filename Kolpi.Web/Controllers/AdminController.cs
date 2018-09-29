using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kolpi.Data;
using Kolpi.Models.ScoreCard;
using Kolpi.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kolpi.Web.Controllers
{
    [Authorize]
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

            var teamScores = _context.TeamScores.Include(t => t.Team).Include(u => u.KolpiUser);
            var teamScoreList = await teamScores.ToListAsync();
            var teamScoreListViewModels = teamScoreList.Select(x => new TeamScoreViewModel(x)).ToList();
            var teamsWithFinalScores = teamScoreList.GroupBy(x => x.Team.TeamName)
                .Select(g => new TeamViewModel
                {
                    TeamName = g.Key,
                    FinalScoreEarned = g.Sum(x => x.InnovationAverageScore) / g.Count(),
                    Theme = Teams.Find(g.Key).Theme
                }).ToList();

            var teamRViewModels = teamsWithFinalScores.OrderByDescending(x => x.FinalScoreEarned).ToList();
            var finalResult = new ResultViewModel { Teams = teamRViewModels, TeamScores = new List<TeamScoreViewModel>()};

            return View(finalResult);
        }

        public IActionResult Error(string errorCode, string message)
        {
            return View(new ErrorViewModel { ErrorCode = errorCode, Message = message });
        }
    }
}

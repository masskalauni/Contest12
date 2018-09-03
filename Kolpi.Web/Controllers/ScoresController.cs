using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Kolpi.Models.ScoreCard;
using Kolpi.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kolpi.Data;

namespace Kolpi.Web.Controllers
{
    [Authorize]
    public class ScoresController : Controller
    {
        private readonly KolpiDbContext _context;

        public ScoresController(KolpiDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var me = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var teamScores = _context.TeamScores.Include(t => t.Team).Where(x => x.KolpiUserId == me);
            var scoreList = await teamScores.ToListAsync();
            return View(scoreList.Select(x => new TeamScoreViewModel(x)));
        }

        public IActionResult Create()
        {
            var model = new TeamScoreViewModel { Team = "Teamcode to make it selected" };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InnovationScore,UsefulnessScore,QualityScore,CompanyValueScore,PresentationScore,Team")] TeamScoreViewModel teamScoreViewModel)
        {
            if (ModelState.IsValid)
            {
                var me = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                //If user already inserted score for this team, don't reinsert inform them
                var record = _context.TeamScores.Where(x => x.Team.TeamCode == teamScoreViewModel.Team && x.KolpiUserId == me).ToList();

                if (record.Any())
                    return RedirectToAction(nameof(Error), new { errorCode = "Duplicate Record", message = $"You aleady evaluated team: {Teams.Find(teamScoreViewModel.Team).TeamName}. Please update their score if you wish to." });

                var teamScore = new TeamScore(teamScoreViewModel)
                {
                    KolpiUserId = me
                };

                _context.Add(teamScore);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            //Something went wrong, keep user on same page
            return View(teamScoreViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(int? id)
        {
            //return View("Error", new ErrorViewModel { ErrorCode = "Score Edit Disabled", Message = "You can't edit scores since 2017-11-30 11:30 AM, all judges already concluded rankings." });

            if (id == null)
            {
                return NotFound();
            }

            var teamScore = await _context.TeamScores.Include(x => x.Team).SingleOrDefaultAsync(m => m.Id == id);
            if (teamScore == null)
            {
                return NotFound($"Record with id {id} does not exist in store.");
            }

            var teamScoreViewModel = new TeamScoreViewModel(teamScore);
            return View(teamScoreViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TeamScoreViewModel teamScoreViewModel)
        {
            if (id != teamScoreViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var teamScore = _context.TeamScores.Find(id);
                    var entity = _context.Attach(teamScore);
                    entity.Entity.Id = id;
                    entity.Entity.InnovationScore = teamScoreViewModel.InnovationScore.Value;
                    entity.Entity.UsefulnessScore = teamScoreViewModel.UsefulnessScore.Value;
                    entity.Entity.CompanyValueScore = teamScoreViewModel.CompanyValueScore.Value;
                    entity.Entity.PresentationScore = teamScoreViewModel.PresentationScore.Value;
                    entity.Entity.QualityScore = teamScoreViewModel.QualityScore.Value;
                    entity.Entity.WeightedAverageScore = teamScoreViewModel.WeightedAverageScore.Value;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(teamScoreViewModel.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teamScoreViewModel);
        }

        public async Task<IActionResult> DeleteAsync(int? id)
        {
            //return View("Error", new ErrorViewModel { ErrorCode = "Score Delete Disabled", Message = "You can't delete scores since 2017-11-30 11:30 AM, all judges already concluded rankings." });

            if (id == null)
            {
                return NotFound();
            }

            var teamScore = await _context.TeamScores
                .Include(t => t.Team)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (teamScore == null)
            {
                return NotFound();
            }

            return View(new TeamScoreViewModel(teamScore));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamScore = await _context.TeamScores.SingleOrDefaultAsync(m => m.Id == id);
            _context.TeamScores.Remove(teamScore);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult About()
        {
            ViewData["Title"] = "About";
            return View();
        }

        public IActionResult Error(string errorCode, string message)
        {
            if (errorCode == "Duplicate Record")
                return View(new ErrorViewModel { ErrorCode = errorCode, Message = message });

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool TeamExists(int id)
        {
            return _context.TeamScores.Any(e => e.Id == id);
        }
    }
}

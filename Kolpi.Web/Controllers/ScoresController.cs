using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Kolpi.Models.Score;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kolpi.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Kolpi.Web.Models;
using System;
using Kolpi.Web.Constants;
using Kolpi.Web.Common;

namespace Kolpi.Web.Controllers
{
    [Authorize]
    public class ScoresController : Controller
    {
        private readonly KolpiDbContext _context;
        private readonly IConfiguration _configuration;

        public ScoresController(KolpiDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var me = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var teamScores = _context.JudgeScores.Where(x => x.Team.CreatedOn.IsCurrentYear()).Include(t => t.Team).Where(x => x.KolpiUserId == me);
            var scoreList = await teamScores.ToListAsync();

            var editSetting = await _context.Settings.FirstOrDefaultAsync(x => x.Name==Setting.ScoreEdit);
            ViewData["AllowScoreEdit"] = editSetting?.Value == "1";

            return View(scoreList.Select(x => new JudgeScoreViewModel(x)));
        }

        public async Task<IActionResult> Create()
        {
            var teams = await _context.Teams.Where(x => x.CreatedOn.IsCurrentYear()).ToListAsync();
            var teamSelectlist = teams.Select(x => new SelectListItem { Value = x.TeamCode, Text = $"{x.TeamName} ( {x.Theme.ToString()} )" }).ToList();

            var model = new JudgeScoreViewModel { Teams = teamSelectlist };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InnovationScore,UsefulnessScore,QualityScore,CompanyValueScore,PresentationScore,Team")] JudgeScoreViewModel teamScoreViewModel)
        {
            if (ModelState.IsValid)
            {
                var me = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                //If user already inserted score for this team, don't reinsert inform them
                var record = await _context.JudgeScores.Where(x => x.Team.TeamCode == teamScoreViewModel.Team && x.KolpiUserId == me).ToListAsync();
                var team = await _context.Teams.FirstAsync(x => x.TeamCode.Equals(teamScoreViewModel.Team));

                if (record.Any())
                    return RedirectToAction(nameof(HomeController.Error), "Home", new { errorCode = "Duplicate Record", message = $"You aleady evaluated team: {team.TeamName}. Please update their score if you wish to." });

                var teamScore = new JudgeScore(teamScoreViewModel)
                {
                    KolpiUserId = me,
                    Team = team
                };

                _context.Add(teamScore);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            //Something went wrong, keep user on same page
            return View(teamScoreViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var me = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var teamScore = await _context.JudgeScores.Include(x => x.Team).SingleOrDefaultAsync(m => m.Id == id && m.KolpiUserId == me);
            if (teamScore == null)
            {
                return NotFound($"Record with id {id} does not exist in store.");
            }

            var teamScoreViewModel = new JudgeScoreViewModel(teamScore);

            return View(teamScoreViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, JudgeScoreViewModel teamScoreViewModel)
        {
            var editSetting = await _context.Settings.FirstOrDefaultAsync(x => x.Name == Setting.ScoreEdit);
            var allowEditing = editSetting?.Value == "1";

            if (!allowEditing)
                return View("Error", new ErrorViewModel { ErrorCode = "Score Edit Disabled", Message = "All judges already concluded their scores, you can't edit." });

            if (id != teamScoreViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var teamScore = _context.JudgeScores.Find(id);
                    var entity = _context.Attach(teamScore);
                    entity.Entity.Id = id;
                    entity.Entity.InnovationScore = teamScoreViewModel.InnovationScore.Value;
                    entity.Entity.UsefulnessScore = teamScoreViewModel.UsefulnessScore.Value;
                    entity.Entity.CompanyValueScore = teamScoreViewModel.CompanyValueScore.Value;
                    entity.Entity.PresentationScore = teamScoreViewModel.PresentationScore.Value;
                    entity.Entity.QualityScore = teamScoreViewModel.QualityScore.Value;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScoreExists(teamScoreViewModel.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teamScoreViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            //return View("Error", new ErrorViewModel { ErrorCode = "Score Delete Disabled", Message = "You can't delete scores since 2017-11-30 11:30 AM, all judges already concluded rankings." });

            if (id == null)
            {
                return NotFound();
            }

            var teamScore = await _context.JudgeScores
                .Include(t => t.Team)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (teamScore == null)
            {
                return NotFound();
            }

            return View(new JudgeScoreViewModel(teamScore));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamScore = await _context.JudgeScores.SingleOrDefaultAsync(m => m.Id == id);
            _context.JudgeScores.Remove(teamScore);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult About()
        {
            ViewData["Title"] = "About";
            return View();
        }

        private bool ScoreExists(int id)
        {
            return _context.JudgeScores.Any(e => e.Id == id);
        }
    }
}

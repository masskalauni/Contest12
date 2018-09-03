using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kolpi.Data;
using Kolpi.Models.ScoreCard;
using System.Security.Claims;

namespace Kolpi.Web.Controllers
{
    public class TeamsController : Controller
    {
        private readonly KolpiDbContext _context;

        public TeamsController(KolpiDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var teams = await _context.Teams.Include(team => team.Participants).ToListAsync();
            var teamViewModels = teams.Select(x => new TeamViewModel(x));
            return View(teamViewModels);
        }
        
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // Specific properties only: To protect from overposting attacks
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeamName,Theme,ProblemStatement,ITRequirements,OtherRequirements,Participants")] TeamViewModel teamViewModel)
        {            
            if (ModelState.IsValid)
            {
                if (!ParticipantsEnteredCorrectly(teamViewModel.Participants))
                    return View(teamViewModel);

                teamViewModel.CreatedBy = User.FindFirst(ClaimTypes.Name)?.Value ?? "";

                _context.Add(new Team(teamViewModel));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teamViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            return View(new TeamViewModel(team));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamName,Theme,ProblemStatement,ITRequirements,OtherRequirements,Participants")] TeamViewModel teamViewModel)
        {
            if (id != teamViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!ParticipantsEnteredCorrectly(teamViewModel.Participants))
                    return View(teamViewModel);

                try
                {
                    _context.Update(teamViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamViewModelExists(teamViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teamViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(new TeamViewModel(team));
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamViewModelExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }

        private bool ParticipantsEnteredCorrectly(string participantsPosted)
        {
            var participants = participantsPosted.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var participant in participants)
            {
                var properties = participant.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (properties.Length != 4)  //User required to enter exact 4 comma separated values for each participant
                {
                    ModelState.AddModelError("Participants", "Please enter participants exactly as given example below: Participant must be in new line each and should have exactly 5 values separated by commas.");
                    return false;
                }
            }

            return true;
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kolpi.Data;
using Kolpi.Models.ScoreCard;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Kolpi.Utilities;
using Microsoft.AspNetCore.Authorization;
using Kolpi.Web.Constants;

namespace Kolpi.Web.Controllers
{
    [Authorize(Roles = Role.AdminOrCommitteeOrParticipant)]
    public class TeamsController : Controller
    {
        private readonly KolpiDbContext _context;

        public TeamsController(KolpiDbContext context)
        {
            _context = context;
        }

        //[Authorize(Roles = Role.AdminOrCommittee)]
        public async Task<IActionResult> Index()
        {
            var teams = await _context.Teams.Include(team => team.Participants).ToListAsync();
            var currentUserName = User.FindFirst(ClaimTypes.Name)?.Value;
            var teamViewModels = teams.Select(x => new TeamViewModel(x, currentUserName));
            return View(teamViewModels);
        }

        public async Task<IActionResult> Analytics()
        {

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // Specific properties only: To protect from overposting attacks
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeamName,Theme,ProblemStatement,ITRequirements,OtherRequirements,Participants")] TeamViewModel teamViewModel)
        {
            if (ModelState.IsValid)
            {
                if (!ParticipantsEnteredCorrectly(teamViewModel.Participants))
                    return View(teamViewModel);

                //Raw participants in text are good to deserialize
                var participantsDtos = DeserializeParticipants(teamViewModel.Participants);
                var partcipantsOnDb = _context.Participants.ToList();

                var alreadyTakenParticipants = partcipantsOnDb.Intersect(participantsDtos, ComplexTypeComparer<Participant>.Create(x => x.Inumber));
                if (alreadyTakenParticipants.Any())
                {
                    ModelState.AddModelError("Participants", $"Participants submitted are already involved with other teams: {string.Join(", ", alreadyTakenParticipants.Select(x => x.Name))}");
                    return View(teamViewModel);
                }

                //Autogenerate properties at creation time
                teamViewModel.CreatedBy = User.FindFirst(ClaimTypes.Name)?.Value ?? "";
                teamViewModel.CreatedOn = DateTime.Now;
                teamViewModel.TeamCode = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
                

                _context.Add(new Team(teamViewModel, participantsDtos));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teamViewModel);
        }

        public async Task<IActionResult> Edit(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
            {
                return NotFound();
            }

            var team = await _context.Teams.SingleAsync(t => t.TeamCode == identifier);            

            if (team == null)
            {
                return NotFound();
            }

            //Fetch participants explicitly
            await _context.Entry(team).Collection(t => t.Participants).LoadAsync();

            return View(new TeamViewModel(team));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string identifier, [Bind("Id,TeamCode,TeamName,Theme,ProblemStatement,ITRequirements,RepoUrl,OtherRequirements,Participants")] TeamViewModel teamViewModel)
        {
            if (identifier != teamViewModel.TeamCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!ParticipantsEnteredCorrectly(teamViewModel.Participants))
                {
                    return View(teamViewModel);
                }
                
                //Raw participants in text are good to deserialize
                var participantsDtos = DeserializeParticipants(teamViewModel.Participants);
                var team = new Team(teamViewModel, participantsDtos);

                var oldParticipants = _context.Participants.Where(x => x.Team.Id == team.Id);
                _context.Participants.RemoveRange(oldParticipants);

                try
                {   
                    _context.Attach(team);

                    _context.Entry(team).Property(p => p.TeamName).IsModified = true;
                    _context.Entry(team).Property(p => p.ProblemStatement).IsModified = true;
                    _context.Entry(team).Property(p => p.Theme).IsModified = true;
                    _context.Entry(team).Collection(p => p.Participants).IsModified = true;
                    _context.Entry(team).Property(p => p.ITRequirements).IsModified = true;
                    _context.Entry(team).Property(p => p.OtherRequirements).IsModified = true;
                    _context.Entry(team).Property(p => p.RepoUrl).IsModified = true;
                                        
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

        public async Task<IActionResult> Delete(string identifier)
        {
            if (identifier == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.SingleAsync(m => m.TeamCode == identifier);
            if (team == null)
            {
                return NotFound();
            }

            //Fetch participants explicitly
            await _context.Entry(team).Collection(t => t.Participants).LoadAsync();

            return View(new TeamViewModel(team));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int identifier)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(x => x.TeamCode.Equals(identifier));
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
                //User required to enter exact 5 comma separated values for each participant
                if (properties.Length != 5)  
                {
                    ModelState.AddModelError("Participants", $"Please enter participants exactly as example says. Each participant in new line and have exactly 5 values separated by commas. Btw, '{participant}' causing this error.");
                    return false;
                }                
            }
            
            return true;
        }

        private IList<Participant> DeserializeParticipants(string participants)
        {            
            return participants.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => new Participant(x.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))).ToList();
        }
    }
}

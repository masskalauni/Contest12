using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kolpi.Data;
using Kolpi.Models.Score;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Kolpi.Web.Constants;

namespace Kolpi.Web.Controllers
{
    [Authorize(Roles = Role.SuperAdmin)]
    public class ConfigurationController : Controller
    {
        private readonly KolpiDbContext _context;

        public ConfigurationController(KolpiDbContext context)
        {
            _context = context;
        }

        // GET: Configuration
        public async Task<IActionResult> Index()
        {
            return View(await _context.Settings.ToListAsync());
        }

        // GET: Configuration/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settings = await _context.Settings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (settings == null)
            {
                return NotFound();
            }

            return View(settings);
        }

        // GET: Configuration/Create
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Value")] Settings settings)
        {
            if (ModelState.IsValid)
            {
                settings.DateCreated = DateTime.Now;
                settings.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                _context.Add(settings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(settings);
        }

        // GET: Configuration/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settings = await _context.Settings.FindAsync(id);
            if (settings == null)
            {
                return NotFound();
            }
            return View(settings);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Value")] Settings settings)
        {
            if (id != settings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    settings.LastUpdated = DateTime.Now;
                    settings.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                    var entry = _context.Attach(settings);
                    entry.Property(x => x.Name).IsModified = true;
                    entry.Property(x => x.Value).IsModified = true;
                    entry.Property(x => x.LastUpdated).IsModified = true;
                    entry.Property(x => x.UpdatedBy).IsModified = true;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SettingsExists(settings.Id))
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
            return View(settings);
        }

        // GET: Configuration/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var settings = await _context.Settings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (settings == null)
            {
                return NotFound();
            }

            return View(settings);
        }

        // POST: Configuration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var settings = await _context.Settings.FindAsync(id);
            _context.Settings.Remove(settings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SettingsExists(int id)
        {
            return _context.Settings.Any(e => e.Id == id);
        }
    }
}

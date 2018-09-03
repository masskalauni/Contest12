using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kolpi.Data;
using Kolpi.Models.Survey;

namespace Kolpi.Web.Controllers
{
    [Authorize]
    public class SurveyController : Controller
    {
        private readonly KolpiDbContext _context;

        public SurveyController(KolpiDbContext context)
        {
            _context = context;
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
    }
}

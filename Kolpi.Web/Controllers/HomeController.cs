using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kolpi.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kolpi.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error(ErrorViewModel errorViewModel)
        {
            return View(new ErrorViewModel { ErrorCode = errorViewModel.ErrorCode, Message = errorViewModel.Message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
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

        public IActionResult Error(string errorCode, string message)
        {
            if (errorCode == "Duplicate Record")
                return View(new ErrorViewModel { ErrorCode = errorCode, Message = message });

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Models;

namespace App.Controllers
{
    public class RoleController : Controller
    {
        [HttpGet]
        public IActionResult Change()
        {
            ViewData["Message"] = "Your changerole page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

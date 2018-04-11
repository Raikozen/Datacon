using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Models;

namespace App.Controllers
{
    public class HomeController : Controller
    {
		public IActionResult Index()
		{
            CheckForLogin();
			return View("Index");
		}

        public void CheckForLogin()
		{
			if(Request.Cookies["userId"] != "" && Convert.ToInt32(Convert.ToInt32(Request.Cookies["userId"])) != 0)
			{
				return;
			}

			Response.Redirect("/User/Login");
		}

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

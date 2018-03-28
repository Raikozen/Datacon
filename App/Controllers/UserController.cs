using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.ViewModels;

namespace App.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View("Create");
        }

		[HttpGet]
		public IActionResult Create()
		{
			return View("Create");
		}

        public IActionResult ContactList()
        {
            return View("ContactList");
        }
    }
}
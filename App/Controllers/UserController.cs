using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.ViewModels;
using App.Repositorys;
using App.Datalayer;
using App.Models;

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
            List<User> users = new UserRepository(new UserSQLContext()).GetUserList();
            return View("ContactList", users);
        }

        [HttpPost]
        public IActionResult Create(UserViewModel viewModel)
        {
            return View();
        }

        public IActionResult ContactList(List<User> _users, string sort)
        {

        }
    }
}
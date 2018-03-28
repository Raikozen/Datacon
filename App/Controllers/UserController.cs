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
            if (ModelState.IsValid)
            {
                UserSQLContext context = new UserSQLContext();
                UserRepository repository = new UserRepository(context);

                if (viewModel.Infix == null)
                {
                    
                    repository.RegisterNoInfix(viewModel.Email, viewModel.Password, viewModel.Firstname, viewModel.Lastname, viewModel.Telnr);
                }
                else
                {
                    repository.RegisterwInfix(viewModel.Email, viewModel.Password, viewModel.Firstname, viewModel.Infix, viewModel.Lastname, viewModel.Telnr);
                }

                return RedirectToAction("Create", "User");
            }

            TempData["Notification"] = "The account with email " + viewModel.Email + "has been created.";

            return View();
        }

        public IActionResult ContactList(List<User> _users, string sort)
        {

        }
    }
}
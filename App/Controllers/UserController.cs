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

        [HttpGet]
        public IActionResult ContactList()
        {
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.users = new UserRepository(new UserSQLContext()).GetUserList().OrderBy(o => o.FullName).ToList();
            userViewModel.sortBy = "Name";
            return View("ContactList", userViewModel);
        }

        [HttpPost]
        public IActionResult ContactList(string sort)
        {
            UserViewModel userViewModel = new UserViewModel();
            List<User> users = new UserRepository(new UserSQLContext()).GetUserList();
            if (sort == "Name")
            {
                userViewModel.users = users.OrderBy(o => o.FullName).ToList();
            }
            else if (sort == "Email Address")
            {
                userViewModel.users = users.OrderBy(o => o.Emailaddress).ToList();
            }
            else if (sort == "Role")
            {
                userViewModel.users = users.OrderBy(o => o.Role.name).ToList();
            }
            userViewModel.sortBy = sort;
            return View("ContactList", userViewModel);
        }
    }
}
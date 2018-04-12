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
    public class UserController : HomeController
    {
		[HttpGet]
		public IActionResult Create()
		{
			base.CheckForLogin();

			return View("Create");
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View("Login");
		}

		[HttpPost]
		public IActionResult Login(LoginViewModel viewModel)
		{
			UserSQLContext context = new UserSQLContext();
			UserRepository repoUser = new UserRepository(context);

			if(ModelState.IsValid)
			{
				User user = repoUser.Login(viewModel.Email, viewModel.Password);

				if(user != null)
				{
					Response.Cookies.Append("userId", Convert.ToString(user.Id));

					return RedirectToAction("Index", "Home");
				}

				ModelState.AddModelError("ErrorMessage", "Invald login credentials");
			}

			return View("Login");
		}

        [HttpPost]
        public IActionResult Create(UserViewModel viewModel)
        {
			base.CheckForLogin();

			if (ModelState.IsValid)
            {
                UserSQLContext context = new UserSQLContext();
                UserRepository repository = new UserRepository(context);

				repository.Register(viewModel.Email, viewModel.Password, viewModel.Firstname, viewModel.Lastname, viewModel.Telnr, viewModel.Infix);

                return RedirectToAction("Create", "User");
            }

            TempData["Notification"] = "The account with email " + viewModel.Email + "has been created.";

            return View();
        }

        [HttpGet]
        public IActionResult ContactList()
        {
			base.CheckForLogin();

			UserViewModel userViewModel = new UserViewModel();
            userViewModel.users = new UserRepository(new UserSQLContext()).GetUserList().OrderBy(o => o.FullName).ToList();
            userViewModel.sortBy = "Name";
            return View("ContactList", userViewModel);
        }

        [HttpPost]
        public IActionResult ContactList(string sort)
        {
			base.CheckForLogin();

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
                userViewModel.users = users.OrderBy(o => o.Role.Name).ToList();
            }
            userViewModel.sortBy = sort;
            return View("ContactList", userViewModel);
        }

		[HttpGet]
		public IActionResult CallInSick()
		{
			base.CheckForLogin();
            int id = Convert.ToInt32(Request.Cookies["userId"]);

            ViewModels.CallInSickViewModel viewModel = new CallInSickViewModel();
			UserRepository userRep = new UserRepository(new UserSQLContext());

            viewModel.isSick = userRep.IsSick(id);
            viewModel.hasOverviewRight = userRep.GetUser(id).Role.Rights.Any(r => r.Id == 10) ? true : false;
            viewModel.SickReportsUser = userRep.GetSickReportsUser(id);
			return View("CallInSick", viewModel);
		}

		[HttpPost]
		public IActionResult CallInSickPost()
		{
			base.CheckForLogin();

			int id = Convert.ToInt32(Request.Cookies["userId"]);

			UserRepository userRep = new UserRepository(new UserSQLContext());
            if (userRep.IsSick(id) == false)
            {
                userRep.ReportSick(id);
            }
            else
            {
                userRep.SicknessRestored(id);
            }

            ViewModels.CallInSickViewModel viewModel = new CallInSickViewModel();
            viewModel.isSick = userRep.IsSick(id);
            viewModel.hasOverviewRight = userRep.GetUser(id).Role.Rights.Any(r => r.Id == 10) ? true : false;
            viewModel.SickReportsUser = userRep.GetSickReportsUser(id);
            return View("CallInSick", viewModel);
		}
	}
}
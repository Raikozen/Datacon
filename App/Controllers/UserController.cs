﻿using System;
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

            if (base.CheckForRight(1))
            {
                return View("Create");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("userId");
            return RedirectToAction("Index", "Home", "");
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

            if (base.CheckForRight(7))
            {
                UserViewModel userViewModel = new UserViewModel();
                userViewModel.users = new UserRepository(new UserSQLContext()).GetUserList().OrderBy(o => o.FullName).ToList();
                userViewModel.sortBy = "Name";
                return View("ContactList", userViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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
            viewModel.SickReportsAll = userRep.GetSickReportsAll() == null ? null : userRep.GetSickReportsAll().OrderBy(o=>o.UserName).ToList();
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
            viewModel.SickReportsAll = userRep.GetSickReportsAll().OrderBy(o => o.UserName).ToList();
            return View("CallInSick", viewModel);
		}

        [HttpGet]
        public IActionResult Holidays()
        {
            base.CheckForLogin();

            UserRepository userRep = new UserRepository(new UserSQLContext());
            HolidaysViewModel holidaysViewModel = new HolidaysViewModel((userRep.GetUser(Convert.ToInt32(Request.Cookies["userId"])).Role.Rights.Any(f=>f.Id == 11) ? true : false),
                userRep.GetAllHolidayRequests(), userRep.GetUnapprovedHolidayRequests(), userRep.GetUserHolidayRequests(Convert.ToInt32(Request.Cookies["userId"])));
            return View(holidaysViewModel);
        }
        
        [HttpPost]
        public IActionResult SubmitRequest(DateTime dateStart, DateTime dateEnd, string description)
        {
            base.CheckForLogin();
            bool approved = false;
            if (new UserRepository(new UserSQLContext()).GetUser(Convert.ToInt32(Request.Cookies["userId"])).Role.Rights.Any(f=>f.Id == 11))
            {
                approved = true;
            }
            HolidayRequest holidayRequest = new HolidayRequest(Convert.ToInt32(Request.Cookies["userId"]), dateStart, dateEnd, description, approved);
            new UserRepository(new UserSQLContext()).AddHolidayRequest(holidayRequest);
            return RedirectToAction("Holidays");
        }

        [HttpPost]
        public IActionResult DeleteRequest(int id)
        {
            base.CheckForLogin();
            new UserRepository(new UserSQLContext()).DeleteHolidayRequest(id);
            return RedirectToAction("Holidays");
        }

        [HttpPost]
        public IActionResult ApproveRequest(int id)
        {
            base.CheckForLogin();
            new UserRepository(new UserSQLContext()).ApproveHolidayRequest(id);
            return RedirectToAction("Holidays");
        }
	}
}
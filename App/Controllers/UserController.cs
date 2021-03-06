﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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


            if(!base.CheckForRight(1))
            {
                return RedirectToAction("Index", "Home");
            }

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
					HttpContext.Session.SetInt32("id", user.Id);
					return RedirectToAction("Index", "Home");
				}

				ModelState.AddModelError("ErrorMessage", "Invald login credentials");
			}

			return View("Login");
		}

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
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
                ConfirmAccount(viewModel);
            }
            return View();
        }

        [HttpGet]
        public IActionResult ContactList()
        {
			base.CheckForLogin();

            if (base.CheckForRight(4))
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

            int id = Convert.ToInt32(HttpContext.Session.GetInt32("id"));

            ViewModels.CallInSickViewModel viewModel = new CallInSickViewModel();
			UserRepository userRep = new UserRepository(new UserSQLContext());

            viewModel.isSick = userRep.IsSick(id);
            viewModel.hasOverviewRight = userRep.GetUser(id).Role.Rights.Any(r => r.Id == 7) ? true : false;
            viewModel.SickReportsUser = userRep.GetSickReportsUser(id);
            viewModel.SickReportsAll = userRep.GetSickReportsAll() == null ? null : userRep.GetSickReportsAll().OrderBy(o=>o.UserName).ToList();
			return View("CallInSick", viewModel);
		}

		[HttpPost]
		public IActionResult CallInSickPost()
		{
			base.CheckForLogin();

			int id = Convert.ToInt32(HttpContext.Session.GetInt32("id"));

			UserRepository userRep = new UserRepository(new UserSQLContext());
            if (userRep.IsSick(id) == false)
            {
                userRep.ReportSick(id);
                ConfirmSick();
            }
            else
            {
                userRep.SicknessRestored(id);
                ConfirmNotSick();
            }

            ViewModels.CallInSickViewModel viewModel = new CallInSickViewModel();
            viewModel.isSick = userRep.IsSick(id);
            viewModel.hasOverviewRight = userRep.GetUser(id).Role.Rights.Any(r => r.Id == 7) ? true : false;
            viewModel.SickReportsUser = userRep.GetSickReportsUser(id);
            viewModel.SickReportsAll = userRep.GetSickReportsAll().OrderBy(o => o.UserName).ToList();
            return View("CallInSick", viewModel);
		}

        [HttpGet]
        public IActionResult Holidays()
        {
            base.CheckForLogin();

            UserRepository userRep = new UserRepository(new UserSQLContext());
            HolidaysViewModel holidaysViewModel = new HolidaysViewModel((userRep.GetUser(Convert.ToInt32(HttpContext.Session.GetInt32("id"))).Role.Rights.Any(f=>f.Id == 8) ? true : false),
                userRep.GetAllHolidayRequests(), userRep.GetUnapprovedHolidayRequests(), userRep.GetUserHolidayRequests(Convert.ToInt32(HttpContext.Session.GetInt32("id"))));
            return View(holidaysViewModel);
        }
        
        [HttpPost]
        public IActionResult SubmitRequest(HolidaysViewModel holidaysViewModel)
        {
            base.CheckForLogin();

            if (ModelState.IsValid)
            {
                bool approved = false;
                if (new UserRepository(new UserSQLContext()).GetUser(Convert.ToInt32(HttpContext.Session.GetInt32("id"))).Role.Rights.Any(f=>f.Id == 8))
                {
                    approved = true;
                }
                if (holidaysViewModel.DateStart != DateTime.MinValue && holidaysViewModel.DateEnd != DateTime.MinValue && holidaysViewModel.DateStart < holidaysViewModel.DateEnd && !string.IsNullOrEmpty(holidaysViewModel.Description))
                {
                    HolidayRequest holidayRequest = new HolidayRequest(Convert.ToInt32(HttpContext.Session.GetInt32("id")), (DateTime)holidaysViewModel.DateStart, (DateTime)holidaysViewModel.DateEnd, holidaysViewModel.Description, approved);
                    new UserRepository(new UserSQLContext()).AddHolidayRequest(holidayRequest);
                    ConfirmHoliday();
                }
                else
                {
                    WrongHoliday();
                }
            }
            UserRepository userRep = new UserRepository(new UserSQLContext());
            holidaysViewModel.HasApproveHolidayRight = (userRep.GetUser(Convert.ToInt32(HttpContext.Session.GetInt32("id"))).Role.Rights.Any(f => f.Id == 8) ? true : false);
            holidaysViewModel.AllholidayRequests = userRep.GetAllHolidayRequests();
            holidaysViewModel.UnapprovedholidayRequests = userRep.GetUnapprovedHolidayRequests();
            holidaysViewModel.UserholidayRequests = userRep.GetUserHolidayRequests(Convert.ToInt32(HttpContext.Session.GetInt32("id")));

            return View("Holidays", holidaysViewModel);
        }

        [HttpPost]
        public IActionResult DeleteRequest(int id)
        {
            base.CheckForLogin();
            UserRepository userRep = new UserRepository(new UserSQLContext());
            if (base.CheckForRight(8) || userRep.GetUserHolidayRequests(Convert.ToInt32(HttpContext.Session.GetInt32("id"))).Any(a=>a.Id == id))
            {
                userRep.DeleteHolidayRequest(id);
                return RedirectToAction("Holidays");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult ApproveRequest(int id)
        {
            base.CheckForLogin();

            if (base.CheckForRight(8))
            {
                new UserRepository(new UserSQLContext()).ApproveHolidayRequest(id);
                return RedirectToAction("Holidays");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult DeleteUser()
        {
            base.CheckForLogin();
            if (base.CheckForRight(1))
            {
                UserViewModel userViewModel = new UserViewModel();
                userViewModel.users = new UserRepository(new UserSQLContext()).GetUserList().OrderBy(o => o.FullName).ToList();
                userViewModel.sortBy = "Name";
                return View("DeleteUser", userViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult DeleteUser(string sort)
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
            if (base.CheckForRight(1))
            {
                return View("DeleteUser", userViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult DeleteSelectedUser(int userId)
        {
            base.CheckForLogin();
            if (base.CheckForRight(1) && userId != 5)
            {
                new UserRepository(new UserSQLContext()).DeleteUser(userId);
            }
            return RedirectToAction("DeleteUser");
        }

        private void ConfirmAccount(UserViewModel viewModel)
        {
            if(ModelState.IsValid && viewModel.Email != null)
            {
                ViewData["ConfirmAccount"] = "The account with email " + viewModel.Email + " has been created.";
            }
        }

        private void ConfirmSick()
        {
            ViewData["ConfirmSick"] = "Your status is set to 'Sick'.";
        }

        private void ConfirmNotSick()
        {
            ViewData["ConfirmNotSick"] = "Your status is set to 'No longer sick'.";
        }

        private void ConfirmHoliday()
        {
            ViewData["ConfirmHoliday"] = "Your holiday has been requested.";
        }

        private void WrongHoliday()
        {
            ViewData["WrongHoliday"] = "Please pick a valid time for your holiday.";
        }
	}
}
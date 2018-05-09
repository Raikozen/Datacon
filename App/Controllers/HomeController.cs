using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Datalayer;
using App.Repositorys;
using App.ViewModels;
using App.Controllers;

namespace App.Controllers
{
    public class HomeController : Controller
    {
		public IActionResult Index()
		{
            CheckForLogin();

			NewsFeedController newsFeedController = new NewsFeedController();
			IndexViewModel viewModel = new IndexViewModel(newsFeedController.GetAllNews());

			return View("Index", viewModel);
		}

        public void CheckForLogin()
		{
			if(Request.Cookies["userId"] != "" && Convert.ToInt32(Request.Cookies["userId"]) != 0)
			{
                ViewData["Rights"] = new UserRepository(new UserSQLContext()).GetUser(Convert.ToInt32(Request.Cookies["userId"])).Role.Rights;
				return;
			}

			Response.Redirect("/User/Login");
		}

        public bool CheckForRight(int rightid)
        {
            UserSQLContext context = new UserSQLContext();
            UserRepository userrepository = new UserRepository(context);

            int id = Convert.ToInt32(Request.Cookies["userId"]);

            User user = userrepository.GetUser(id);

            return user.Role.Rights.Any(f => f.Id == rightid);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

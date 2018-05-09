using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Datalayer;
using App.Repositories;
using App.ViewModels;

namespace App.Controllers
{
    public class NewsFeedController : HomeController
    {
		/// <summary>
		/// Get all actiive newsfeed posts
		/// </summary>
		/// <returns></returns>
		[HttpGet]
        public List<NewsfeedPost> GetAllNews()
		{
			NewsfeedRepository repoNews = new NewsfeedRepository(new NewsfeedSQLContext());
			return repoNews.GetAllActiveNewsfeedPosts();
		}

		/// <summary>
		/// Show the new newspost view
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Overview()
		{
			base.CheckForLogin();
			base.CheckForRight(1011);

			NewsfeedRepository repoNews = new NewsfeedRepository(new NewsfeedSQLContext());
			NewsFeedOverviewViewModel viewModel = new NewsFeedOverviewViewModel(repoNews.GetAllNewsfeedPosts());

			return View("Overview", viewModel);
		}

		/// <summary>
		/// Create a new newsfeed item
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public IActionResult New(NewsFeedOverviewViewModel viewModel)
		{
			if(ModelState.IsValid)
			{
				NewsfeedRepository repoNews = new NewsfeedRepository(new NewsfeedSQLContext());
				repoNews.CreateNewsfeedPost(viewModel.Message, viewModel.Date);
			}

			return RedirectToAction("Overview");
		}

		/// <summary>
		/// Delete a newsfeed item
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Delete(int id)
		{
			NewsfeedRepository repoNews = new NewsfeedRepository(new NewsfeedSQLContext());

			repoNews.DeleteNewsfeedPost(id);

			return RedirectToAction("Overview");
		}
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Datalayer;
using App.Repositories;

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
		/// Create a new newsfeed post
		/// </summary>
		/// <returns></returns>
		/// 
		/*
		[HttpPost]
		public IActionResult NewNewsFeed()
		{

		}
		*/
    }
}
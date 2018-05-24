using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Datalayer;
using App.Models;

namespace App.Repositories
{
    public class NewsfeedRepository
    {
		private INewsfeedContext context;

		public NewsfeedRepository(INewsfeedContext context)
		{
			this.context = context;
		}

		public List<NewsfeedPost> GetAllNewsfeedPosts()
		{
			return context.GetAllNewsfeedPosts();
		}

		public List<NewsfeedPost> GetAllActiveNewsfeedPosts()
		{
			return context.GetAllActiveNewsfeedPosts();
		}

		public bool CreateNewsfeedPost(string message, DateTime date)
		{
			return context.CreateNewsfeedPost(message, date);
		}

		public bool UpdateNewsfeedPost(int id, string message, DateTime date)
		{
			return context.UpdateNewsfeedPost(id, message, date);
		}

		public bool DeleteNewsfeedPost(int id)
		{
			return context.DeleteNewsfeedPost(id);
		}

		/// <summary>
		/// Delete all old newsfeed posts
		/// </summary>
		public void DeleteOldNewsFeedPosts()
		{
			context.DeleteOldNewsFeedPosts();
		}
	}
}

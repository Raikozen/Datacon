using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;

namespace App.Datalayer
{
    public interface INewsfeedContext
    {
		List<NewsfeedPost> GetAllNewsfeedPosts();
		List<NewsfeedPost> GetAllActiveNewsfeedPosts();

		bool CreateNewsfeedPost(string message, DateTime date);

		bool UpdateNewsfeedPost(int id, string message, DateTime date);

		bool DeleteNewsfeedPost(int id);

		void DeleteOldNewsFeedPosts();
	}
}

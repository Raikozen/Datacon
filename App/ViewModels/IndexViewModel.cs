using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;

namespace App.ViewModels
{
    public class IndexViewModel
    {
		public List<NewsfeedPost> NewsFeedPosts { get; set; }

		public IndexViewModel(List<NewsfeedPost> newsFeedPosts)
		{
			this.NewsFeedPosts = newsFeedPosts;
		}
    }
}

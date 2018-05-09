using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using App.Models;

namespace App.ViewModels
{
    public class NewsFeedOverviewViewModel
    {
		[Required]
		public DateTime Date { get; set; }
		[Required]
		public string Message { get; set; }

		public List<NewsfeedPost> NewsFeedPosts { get; }

		public NewsFeedOverviewViewModel()
		{

		}

		public NewsFeedOverviewViewModel(List<NewsfeedPost> newsFeedPosts)
		{
			this.NewsFeedPosts = newsFeedPosts;
		}
    }
}

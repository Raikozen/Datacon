using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class NewsfeedPost
    {
		private DateTime date;

		public int Id { get; }
		public string Message { get; }
		public string Date
		{
			get
			{
				int day = date.Day;
				int month = date.Month;
				int year = date.Year;
				string result = $"{day}-{month}-{year}";

				return result;
			}
		}

		public NewsfeedPost(int id, string message, DateTime date)
		{
			Id = id;
			Message = message;
			this.date = date;
		}
	}
}

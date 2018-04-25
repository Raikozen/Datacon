using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class ApiHoliday
    {
		public int Id { get; }
		public string DateFrom { get; }
		public string DateTo { get; }
		public string Description { get; }
		public int TotalHours { get; }
		public string Status { get; }

		public ApiHoliday(int id, string dateFrom, string dateTo, string description, int totalHours, string status)
		{
			this.Id = id;
			this.DateFrom = dateFrom;
			this.DateTo = dateTo;
			this.Description = description;
			this.TotalHours = totalHours;
			this.Status = status;
		}
	}
}

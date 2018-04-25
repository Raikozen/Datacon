using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class ApiSickReport
    {
		public int Id { get; }
		public string Email { get; }
		public string Description { get; }
		public string DatetimeStart { get; }

		public ApiSickReport(int id, string email, string description, string DatetimeStart)
		{
			this.Id = id;
			this.Email = email;
			this.Description = description;
			this.DatetimeStart = DatetimeStart;
		}
	}
}

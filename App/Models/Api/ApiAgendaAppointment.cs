using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class ApiAgendaAppointment
    {
		public int Id { get; }
		public string Email { get; }
		public string RoomName { get; }
		public string Description { get; }
		public string Date { get; }

		public ApiAgendaAppointment(int id, string email, string roomName, string description, string date)
		{
			this.Id = id;
			this.Email = email;
			this.RoomName = roomName;
			this.Description = description;
			this.Date = date;
		}
	}
}

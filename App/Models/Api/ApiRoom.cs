using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class ApiRoom
    {
		public int Id { get; }
		public string RoomName { get; }

		public ApiRoom(int id, string roomName)
		{
			Id = id;
			RoomName = roomName;
		}
	}
}

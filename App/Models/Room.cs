using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    class Room
    {
        public int Id { get; }
        public string RoomName { get; }

        public Room(int id, string roomName)
        {
            Id = id;
            RoomName = roomName;
        }
    }
}

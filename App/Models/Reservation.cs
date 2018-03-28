﻿using Proftaak_portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proftaak_portal
{
    class Reservation
    {
        public int Id { get; }
        public User User { get; }
        public Room Room { get; }
        public string ReservationName { get; }
        public DateTime ReservationStart { get; }
        public DateTime ReservationEnd { get; }

        public Reservation(int id, User user, Room room, string reservationName, DateTime reservationStart, DateTime reservationEnd)
        {
            Id = id;
            User = user;
            Room = room;
            ReservationName = reservationName;
            ReservationStart = reservationStart;
            ReservationEnd = reservationEnd;
        }

        public override string ToString()
        {
            return ReservationName + " by " + User.Lastname + " (" + ReservationStart.ToShortDateString() + " " + ReservationStart.ToShortTimeString() +
            " - " + ReservationEnd.ToShortDateString() + " " + ReservationEnd.ToShortTimeString() + ")";
        }
    }
}

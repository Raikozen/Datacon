using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using App.Models;

namespace App.ViewModels
{
    public class ReservationViewModel
    {
        public int Id { get; }
        public User User { get; }
        //public Room Room { get; }
        public string ReservationName { get; }
        public DateTime ReservationStart { get; }
        public DateTime ReservationEnd { get; }
    }
}

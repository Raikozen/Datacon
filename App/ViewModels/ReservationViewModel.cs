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
        //List of current Reservations
        public List<Reservation> Reservations { get; set; }
        //Id of the reservation
        public int Id { get; set; }
        //User object of who created the reservation
        public User User { get; set; }
        //Room object of where the reservation is located.
        public int RoomId { get; set;}
        //Name of Reservation
        [Required(ErrorMessage ="Name of Reservation is required")]
        [Display(Name ="ReservationName", Prompt ="Name of Reservation")]
        public string ReservationName { get; set; }
        //Starttime and date of Reservation
        [Required(ErrorMessage ="Start time of Reservation is required.")]
        [Display(Name ="ReservationStart")]
        public DateTime ReservationStart { get; set; }

        internal void AddRoomId(int roomId)
        {
            this.RoomId = roomId;
        }

        //Endtime and date of Reservation
        [Required(ErrorMessage = "End time of Reservation is required.")]
        [Display(Name ="ReservationEnd")]
        public DateTime ReservationEnd { get; set; }

        internal void AddReservationList(List<Reservation> reservations)
        {
            this.Reservations = reservations;
        }

        public ReservationViewModel()
        {

        }
    }
}

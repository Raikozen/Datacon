using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using App.Datalayer;
using App.Repositorys;
using App.ViewModels;

namespace App.Controllers
{
    public class ReservationController : HomeController
    {
        [HttpGet]
        public IActionResult LoadData()
        {
            base.CheckForLogin();

            List<Room> rooms = new ReservationRepository(new ReservationSQLContext()).GetRooms();
            return View("Reserve", rooms);
        }

        [HttpGet]
        public IActionResult LoadReservations(int roomId)
        {
            base.CheckForLogin();
            HttpContext.Session.SetInt32("roomID", roomId);
            Room room = new Room(Convert.ToInt32(HttpContext.Session.GetInt32("roomID")), "test");
            ReservationRepository repo = new ReservationRepository(new ReservationSQLContext());
            List<Reservation> reservations = repo.GetReservations(room);

            ReservationViewModel viewmodel = new ReservationViewModel();
            viewmodel.Reservations = (reservations);
            viewmodel.AddRoomId(roomId);
            return View("ReserveRoom", viewmodel);
        }

        [HttpGet]
        public IActionResult Transfer()
        {
            base.CheckForLogin();

            return View("ReserveRoom");
        }

        [HttpGet]
        public IActionResult DeleteReservation(int RoomId, int reservationId)
        {
            base.CheckForLogin();
            if (!base.CheckForRight(3))
            {
                return RedirectToAction("Index", "Home");
            }

            ReservationRepository repo = new ReservationRepository(new ReservationSQLContext());
            repo.DeleteReservation(reservationId);
            List<Room> rooms = new ReservationRepository(new ReservationSQLContext()).GetRooms();
            return RedirectToAction("LoadReservations", new { roomId = RoomId });
        }

        [HttpPost]
        public IActionResult AddReservation(ReservationViewModel ViewModel)
        {
			base.CheckForLogin();
			ReservationRepository repo = new ReservationRepository(new ReservationSQLContext());
            List<Reservation> reservations = repo.GetReservations(new Room(ViewModel.RoomId, ""));
            int userId = Convert.ToInt32(HttpContext.Session.GetInt32("id"));
            if (ViewModel.ReservationStart < ViewModel.ReservationEnd && reservations.Any(r => (ViewModel.ReservationStart > r.ReservationStart && ViewModel.ReservationStart < r.ReservationEnd) || (ViewModel.ReservationEnd < r.ReservationEnd && ViewModel.ReservationEnd > r.ReservationStart) || (ViewModel.ReservationStart < r.ReservationStart && ViewModel.ReservationEnd > r.ReservationEnd)) == false)
            {
                repo.AddReservation(ViewModel.RoomId, userId, ViewModel.ReservationName, ViewModel.ReservationStart, ViewModel.ReservationEnd);
                ConfirmReservation();
            }
            else
            {
                WrongReservation();
            }
            List<Room> rooms = new ReservationRepository(new ReservationSQLContext()).GetRooms();
        
            return RedirectToAction("LoadReservations", new { roomId = ViewModel.RoomId });
        }

        private void ConfirmReservation()
        {
            TempData["ConfirmReservation"] = "Your reservation has been added";          
        }

        private void WrongReservation()
        {
            TempData["WrongReservation"] = "Please pick a valid time for your reservation.";
        }
    }
}
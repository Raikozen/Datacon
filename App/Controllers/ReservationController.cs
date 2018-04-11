using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Datalayer;
using App.Repositorys;
using App.ViewModels;

namespace App.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult LoadData()
        {
            List<Room> rooms = new ReservationRepository(new ReservationSQLContext()).GetRooms();
            return View("Reserve", rooms);
        }

        [HttpGet]
        public IActionResult LoadReservations(int roomId)
        {
            Room room = new Room(roomId,"test");
            ReservationRepository repo = new ReservationRepository(new ReservationSQLContext());
            List<Reservation> reservations = repo.GetReservations(room);
           
            ReservationViewModel viewmodel = new ReservationViewModel();
            viewmodel.AddReservationList(reservations);
            viewmodel.AddRoomId(roomId);
            return View("ReserveRoom", viewmodel);
        }

        [HttpGet]
        public IActionResult Transfer()
        {
            
            return View("ReserveRoom");
        }
        
        [HttpGet]
        public IActionResult DeleteReservation(int reservationId)
        {
            ReservationRepository repo = new ReservationRepository(new ReservationSQLContext());
            repo.DeleteReservation(reservationId);
            List<Room> rooms = new ReservationRepository(new ReservationSQLContext()).GetRooms();
            return View("Reserve", rooms);
        }

        [HttpPost]
        public IActionResult AddReservation(ReservationViewModel ViewModel, int userId)
        {
            ReservationRepository repo = new ReservationRepository(new ReservationSQLContext());
            repo.AddReservation(ViewModel.RoomId, userId ,ViewModel.ReservationName, ViewModel.ReservationStart, ViewModel.ReservationEnd);

            return View("ReserveRoom");
        }
    }
}
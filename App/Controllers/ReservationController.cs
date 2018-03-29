﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Datalayer;
using App.Repositorys;

namespace App.Controllers
{
    public class ReservationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RoomList()
        {
            List<Room> rooms = new ReservationRepository(new ReservationSQLContext()).GetRooms();
            return View("Reserve", rooms);
        }

        
    }
}
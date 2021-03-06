﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Repositorys;
using App.Datalayer;

namespace App.Models
{
    public class HolidayRequest
    {
        public int Id { get; }
        public int UserId { get; }
        public User User { get; }
        public DateTime DateStart { get; }
        public DateTime DateEnd { get; }
        public string Description { get; }
        public bool Approved { get; }

        public HolidayRequest(int UserId, DateTime DateStart, DateTime DateEnd, string Description, bool Approved)
        {
            this.UserId = UserId;
            this.DateStart = DateStart;
            this.DateEnd = DateEnd;
            this.Description = Description;
            this.Approved = Approved;
            this.User = new UserRepository(new UserSQLContext()).GetUser(UserId);
        }

        public HolidayRequest(int Id, int UserId, DateTime DateStart, DateTime DateEnd, string Description, bool Approved)
        {
            this.Id = Id;
            this.UserId = UserId;
            this.DateStart = DateStart;
            this.DateEnd = DateEnd;
            this.Description = Description;
            this.Approved = Approved;
            this.User = new UserRepository(new UserSQLContext()).GetUser(UserId);
        }
    }
}

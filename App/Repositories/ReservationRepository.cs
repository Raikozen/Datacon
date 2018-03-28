using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proftaak_portal.Repositorys
{
    class ReservationRepository
    {
        private readonly IReservationContext _Context;

        public ReservationRepository(IReservationContext Context)
        {
            _Context = Context;
        }

        public List<Room> GetRooms()
        {
            return _Context.GetRooms();
        }

        public List<Reservation> GetReservations(Room room)
        {
            return _Context.GetReservations(room);
        }

        public void AddReservation(int roomId, int userId, string reservationName, DateTime reservationStart, DateTime reservationEnd)
        {
            _Context.AddReservation(roomId, userId, reservationName, reservationStart, reservationEnd);
        }

        public void DeleteReservation(int reservationId)
        {
            _Context.DeleteReservation(reservationId);
        }
    }
}

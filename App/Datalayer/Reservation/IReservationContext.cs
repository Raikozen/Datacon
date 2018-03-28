using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proftaak_portal
{
    interface IReservationContext
    {
        List<Room> GetRooms();

        List<Reservation> GetReservations(Room room);

        void AddReservation(int roomId, int userId, string reservationName, DateTime reservationStart, DateTime reservationEnd);

        void DeleteReservation(int reservationId);
    }
}

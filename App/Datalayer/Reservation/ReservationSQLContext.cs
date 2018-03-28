using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proftaak_portal.Datalayer;

namespace Proftaak_portal
{
    class ReservationSQLContext : IReservationContext
    {
        private static readonly string connnectionstring = "Server = mssql.fhict.local; Database = dbi338912; User Id = dbi338912; Password = StealYoBike!";

        /// <summary>
        /// Get a list from all of the rooms
        /// </summary>
        /// <returns></returns>
        public List<Room> GetRooms()
        {
            string query = "SELECT proftaak.Room.id, proftaak.Room.roomName FROM proftaak.Room";
            SqlCommand command = new SqlCommand(query, new SqlConnection(connnectionstring));
            using (command)
            {
                List<Room> rooms = new List<Room>();
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rooms.Add(new Room((int)reader["id"], (string)reader["roomName"]));
                    }
                }
                command.Connection.Close();
                return rooms;
            }
        }

        /// <summary>
        /// Get a list of all the reservations
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public List<Reservation> GetReservations(Room room)
        {
            string query = "SELECT proftaak.Reservation.id, proftaak.Reservation.userId, proftaak.Reservation.roomId, " +
                           "proftaak.Reservation.reservationName, proftaak.Reservation.reservationStart, proftaak.Reservation.reservationEnd " +
                           "FROM proftaak.Reservation WHERE proftaak.Reservation.roomId = @roomId";
            SqlCommand command = new SqlCommand(query, new SqlConnection(connnectionstring));
            command.Parameters.AddWithValue("@roomId", room.Id);
            using (command)
            {
                List<Reservation> reservations = new List<Reservation>();
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User tempUser = new UserSQLContext().GetUser((int)reader["userId"]);
                        reservations.Add(new Reservation((int)reader["id"], tempUser, room, (string)reader["reservationName"],
                                              (DateTime)reader["reservationStart"], (DateTime)reader["reservationEnd"]));
                    }
                }
                command.Connection.Close();
                return reservations;
            }
        }

        /// <summary>
        /// Add a reservation to the system
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="userId"></param>
        /// <param name="reservationName"></param>
        /// <param name="reservationStart"></param>
        /// <param name="reservationEnd"></param>
        public void AddReservation(int roomId, int userId, string reservationName, DateTime reservationStart, DateTime reservationEnd)
        {
            string query = "INSERT INTO proftaak.Reservation (userId, roomId, reservationName, reservationStart, reservationEnd) " +
                           "VALUES (@userId, @roomId, @reservationName, @reservationStart, @reservationEnd)";
            SqlCommand command = new SqlCommand(query, new SqlConnection(connnectionstring));
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@roomId", roomId);
            command.Parameters.AddWithValue("@reservationName", reservationName);
            command.Parameters.AddWithValue("@reservationStart", reservationStart);
            command.Parameters.AddWithValue("@reservationEnd", reservationEnd);

            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        /// <summary>
        /// Delete a reservation
        /// </summary>
        /// <param name="reservationId"></param>
        public void DeleteReservation(int reservationId)
        {
            string query = "DELETE FROM proftaak.Reservation WHERE id = @reservationId";
            SqlCommand command = new SqlCommand(query, new SqlConnection(connnectionstring));
            command.Parameters.AddWithValue("@reservationId", reservationId);

            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }
    }
}

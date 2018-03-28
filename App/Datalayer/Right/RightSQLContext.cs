using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proftaak_portal.Models;
using System.Data.SqlClient;
using System.Data;


namespace Proftaak_portal.Datalayer
{
    

    class RightSQLContext : IRightContext
    {
        private SqlConnection connection;

        public RightSQLContext()
        {
            connection = new SqlConnection("Server = mssql.fhict.local; Database = dbi338912; User Id = dbi338912; Password = StealYoBike!");
        }

        public List<Models.Right> GetRights()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT proftaak.[Right].id as id, proftaak.[Right].name as name FROM proftaak.[Right]; ";

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<Models.Right> rights = new List<Models.Right>();

            while (reader.Read())
            {
                rights.Add(new Models.Right((int)reader["id"], reader["name"].ToString()));
            }

            return rights;

        }
    }


}

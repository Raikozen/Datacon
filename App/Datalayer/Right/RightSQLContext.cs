using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;
using System.Data.SqlClient;
using System.Data;


namespace App.Datalayer
{
    

    class RightSQLContext : IRightContext
    {
        private SqlConnection connection;

        public RightSQLContext()
        {
            connection = new SqlConnection("Server=tcp:proftaaks2.database.windows.net,1433;Initial Catalog=Datacon;Persist Security Info=False;User ID=adminuser;Password=StealY0Bike!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        public List<Models.Right> GetRights()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT dbo.[Right].id as id, dbo.[Right].name as name FROM dbo.[Right]; ";

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

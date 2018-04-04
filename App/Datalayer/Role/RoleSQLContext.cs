using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;

namespace App.Datalayer
{
    public class RoleSQLContext : IRoleContext
    {
        private SqlConnection connection;

        public RoleSQLContext()
        {
            connection = new SqlConnection("Server = mssql.fhict.local; Database = dbi338912; User Id = dbi338912; Password = StealYoBike!");
        }

        /// <summary>
        /// Updates all rights that are connected to the specific role.
        /// </summary>
        public void UpdateRoleRights(int roleID, List<int> rightIDs)
        {
            string query =
                string.Format(
                    "UPDATE proftaak.[Role_Right]" +
                    "SET proftaak.[Role_Right].hasRight = 0" +
                    "WHERE proftaak.[Role_Right].roleId = @roleId;" +
                    "UPDATE proftaak.[Role_Right]" +
                    "SET proftaak.[Role_Right].hasRight = 1" +
                    "WHERE proftaak.[Role_Right].roleId = @roleId " +
                    "AND rightId IN ({0})",
                    string.Join(",", rightIDs));

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@roleId", roleID);

            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        /// <summary>
        /// Get the roles from the database
        /// </summary>
        /// <returns>
        /// List<Role> roles
        /// </returns>
        public List<Role> GetRoles()
        {
            string query =
                "SELECT " +
                "proftaak.[Role_Right].roleId as roleId, " +
                "proftaak.[Role_Right].rightId as rightId, " +
                "proftaak.[Role_Right].hasRight as hasRight, " +
                "proftaak.[Role].name as roleName, " +
                "proftaak.[Right].name as rightName " +
                "FROM " +
                "proftaak.[Role_Right] " +
                "INNER JOIN proftaak.[Role] on proftaak.[Role].id = roleId " +
                "INNER JOIN proftaak.[Right] on proftaak.[Right].id = rightId " +
                "WHERE [Role_Right].hasRight = 1 " +
                "ORDER BY roleId, rightId ";

            SqlCommand command = new SqlCommand(query, connection);

            command.Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<Models.Role> roles = new List<Models.Role>();
            List<Models.Right> rights = new List<Models.Right>();

            int lastRoleId = 0;
            string lastRoleName = null;

            //while reader has rows
            while (reader.Read())
            {
                //if new user role
                if (lastRoleId != (int)reader["roleId"])
                {
                    if (lastRoleId != 0)
                    {
                        //add old role to list
                        roles.Add(new Models.Role(
                            lastRoleId,
                            lastRoleName,
                            rights
                        ));

                        //empty rights array
                        rights = new List<Models.Right>();
                    }

                    lastRoleId = (int)reader["roleId"];
                    lastRoleName = reader["roleName"].ToString();
                }

                //add new right to the list
                rights.Add(new Models.Right(
                    (int)reader["rightId"],
                    reader["rightName"].ToString()
                ));
            }

            //Add the final role to the list
            if (lastRoleId != 0)
            {
                roles.Add(new Models.Role(
                    lastRoleId,
                    lastRoleName,
                    rights
                ));
            }

			connection.Close();
            return roles;
        }

		/*
		public Role GetRoleById(int roleId)
		{
			SqlCommand command = new SqlCommand();
			command.Connection = connection;

			command.CommandText = "SELECT " +
				"proftaak.[Role_Right].roleId as roleId, " +
				"proftaak.[Role_Right].rightId as rightId, " +
				"proftaak.[Role_Right].hasRight as hasRight, " +
				"proftaak.[Role].name as roleName, " +
				"proftaak.[Right].name as rightName " +
				"FROM " +
				"proftaak.[Role_Right] " +
				"INNER JOIN proftaak.[Role] on proftaak.[Role].id = roleId " +
				"INNER JOIN proftaak.[Right] on proftaak.[Right].id = rightId " +
				"WHERE [Role_Right].hasRight = 1 AND [Role].id = @roleId " +
				"ORDER BY roleId, rightId ";

			connection.Open();

			SqlDataReader reader = command.ExecuteReader();

			//while reader has rows
			while (reader.Read())
			{

			}
		}
		*/
    }
}

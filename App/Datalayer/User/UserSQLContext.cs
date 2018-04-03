using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;
using System.Data.SqlClient;
using System.Data;
using App.Repositorys;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace App.Datalayer
{
    public class UserSQLContext : IUserContext
    {
        private SqlConnection connection;

        public UserSQLContext()
        {
            connection = new SqlConnection("Server = mssql.fhict.local; Database = dbi338912; User Id = dbi338912; Password = StealYoBike!");
        }

        /// <summary>
        /// Log a user in
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Login(string emailAddress, string password)
        {
			//Get the salt
			SqlCommand command = new SqlCommand();

			command.Connection = connection;
			command.CommandText = "SELECT salt FROM proftaak.[User] WHERE email = @email";

			command.Parameters.Add("@email", SqlDbType.VarChar);
			command.Parameters["@email"].Value = emailAddress;

			connection.Open();
			SqlDataReader reader = command.ExecuteReader();

			byte[] salt;

			if(reader.HasRows)
			{
				salt = (byte[])reader["salt"];
			} else
			{
				connection.Close();
				return null;
			}

			connection.Close();

			//Hash the entered password with the retrieved salt
			string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: password,
				salt: salt,
				prf: KeyDerivationPrf.HMACSHA256,
				iterationCount: 10000,
				numBytesRequested: 256 / 8
			));

			//Get the user from the database
			command.CommandText = "SELECT " +
				"proftaak.[User].id as userId, proftaak.[User].email as email, proftaak.[User].password as password, proftaak.[User].firstName as firstName, proftaak.[User].lastName as lastName, proftaak.[User].infix as infix, proftaak.[User].telNr as telNr, proftaak.[User].roleId, " +
				"proftaak.[Right].id as rightId, proftaak.[Right].name as rightName, " +
				"proftaak.[Role].name as roleName " +
				"FROM " +
				"proftaak.[User] " +
				"INNER JOIN proftaak.[Role_Right] on proftaak.[Role_Right].roleId = proftaak.[User].roleId " +
				"INNER JOIN proftaak.[Right] on proftaak.[Role_Right].rightId = proftaak.[Right].id " +
				"INNER JOIN proftaak.[Role] on proftaak.[Role_Right].roleId = proftaak.[Role].id " +
				"WHERE proftaak.[Role_Right].hasRight != 0 AND email = @email AND password LIKE @password " +
				"ORDER BY userId";

			command.Parameters.Add("@password", SqlDbType.VarChar);
			command.Parameters["@password"].Value = hashedPassword;

			connection.Open();
			reader = command.ExecuteReader();

			//rights
			List<Right> rights = new List<Right>();
			//role
			int roleId = 0;
			string roleName = "";
			//user
			int userId = 0;
			string firstName = "";
			string lastName = "";
			string email = "";
			string infix = "";
			string telNr = "";

			while (reader.Read())
			{
				//rights
				rights.Add(new Right(
					Convert.ToInt32(reader["rightId"]),
					Convert.ToString(reader["rightName"])
				));

				//role
				roleId = Convert.ToInt32(reader["roleId"]);
				roleName = Convert.ToString(reader["roleName"]);

				//user
				userId = Convert.ToInt32(reader["userId"]);
				firstName = Convert.ToString(reader["firstName"]);
				lastName = Convert.ToString(reader["lastName"]);
				infix = Convert.ToString(reader["infix"]);
				telNr = Convert.ToString(reader["telNr"]);
				email = Convert.ToString(reader["email"]);
			}

			//Check if all the needed data is present
			if (userId != 0 && firstName != "" && lastName != "" && email != "" && roleId != 0 && roleName != "")
			{
				connection.Close();

				//return the user
				return new User(
					userId,
					email,
					firstName,
					lastName,
					infix,
					telNr,
					new Role(roleId, roleName, rights)
				);
			}

			connection.Close();
			return null;
		}


        /// <summary>
        /// Returns a user object by userId
        /// </summary>
        /// <param name="userId"></param>
        public User GetUser(int userId)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT " +
                "proftaak.[User].id as userId, proftaak.[User].email as email, proftaak.[User].password as password, proftaak.[User].firstName as firstName, proftaak.[User].lastName as lastName, proftaak.[User].infix as infix, proftaak.[User].telNr as telNr, proftaak.[User].roleId, " +
                "proftaak.[Right].id as rightId, proftaak.[Right].name as rightName, " +
                "proftaak.[Role].name as roleName " +
                "FROM " +
                "proftaak.[User] " +
                "INNER JOIN proftaak.[Role_Right] on proftaak.[Role_Right].roleId = proftaak.[User].roleId " +
                "INNER JOIN proftaak.[Right] on proftaak.[Role_Right].rightId = proftaak.[Right].id " +
                "INNER JOIN proftaak.[Role] on proftaak.[Role_Right].roleId = proftaak.[Role].id " +
                "WHERE proftaak.[User].id = @userId AND proftaak.[Role_Right].hasRight = 1 " +
                "ORDER BY userId";

            command.Parameters.Add("@userId", SqlDbType.Int);

            command.Parameters["@userId"].Value = userId;

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            //rights
            List<Right> rights = new List<Right>();
            //role
            int roleId = 0;
            string roleName = "";
            //user
            string firstName = "";
            string lastName = "";
            string email = "";
            string infix = "";
            string telNr = "";

            //Store the data
            while (reader.Read())
            {
                //rights
                rights.Add(new Right(
                    Convert.ToInt32(reader["rightId"]),
                    Convert.ToString(reader["rightName"])
                ));

                //role
                roleId = Convert.ToInt32(reader["roleId"]);
                roleName = Convert.ToString(reader["roleName"]);
                
                //User
                firstName = Convert.ToString(reader["firstName"]);
                lastName = Convert.ToString(reader["lastName"]);
                infix = Convert.ToString(reader["infix"]);
                telNr = Convert.ToString(reader["telNr"]);
                email = Convert.ToString(reader["email"]);
            }

            //Create the user object
            if (userId != 0 && firstName != "" && lastName != "" && email != "" && roleId != 0 && roleName != "")
            {
                connection.Close();

                //return the user
                return new User(
                    userId,
                    email,
                    firstName,
                    lastName,
                    infix,
                    telNr,
                    new Role(roleId, roleName, rights)
                );
            }

            connection.Close();
            return null;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        public List<User> GetUserList()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;

            command.CommandText = "SELECT " +
                "proftaak.[User].id as userId, proftaak.[User].email as email, proftaak.[User].password as password, proftaak.[User].firstName as firstName, proftaak.[User].lastName as lastName, proftaak.[User].infix as infix, proftaak.[User].telNr as telNr, proftaak.[User].roleId, " +
                "proftaak.[Right].id as rightId, proftaak.[Right].name as rightName, " +
                "proftaak.[Role].name as roleName " +
                "FROM " +
                "proftaak.[User] " +
                "INNER JOIN proftaak.[Role_Right] on proftaak.[Role_Right].roleId = proftaak.[User].roleId " +
                "INNER JOIN proftaak.[Right] on proftaak.[Role_Right].rightId = proftaak.[Right].id " +
                "INNER JOIN proftaak.[Role] on proftaak.[Role_Right].roleId = proftaak.[Role].id " +
                "WHERE proftaak.[Role_Right].hasRight != 0 " +
                "ORDER BY userId";

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            //Role details
            int roleId = 0;
            string roleName = null;
            //User details
            int userId = 0;
            string email = null;
            string firstName = null;
            string lastName = null;
            string infix = null;
            string telNr = null;

            //The user list
            List<User> userList = new List<User>();
            //List of rights (used in the while loop)
            List<Right> currentRights = new List<Right>();

            while (reader.Read())
            {
                //New user?
                if (Convert.ToInt32(reader["userId"]) != userId && userId != 0)
                {
                    //Create the previous user's role
                    Role currentRole = new Role(
                        Convert.ToInt32(roleId),
                        Convert.ToString(roleName),
                        currentRights
                    );

                    //Add the previous user to the list
                    userList.Add(new User(
                        Convert.ToInt32(userId),
                        Convert.ToString(email),
                        Convert.ToString(firstName),
                        Convert.ToString(lastName),
                        Convert.ToString(infix),
                        Convert.ToString(telNr),
                        currentRole
                    ));

                    //Clear the current rights list
                    currentRights = new List<Right>();
                }

                //Store the role data
                roleId = Convert.ToInt32(reader["roleId"]);
                roleName = Convert.ToString(reader["roleName"]);

                //Store the user data
                userId = Convert.ToInt32(reader["userId"]);
                email = Convert.ToString(reader["email"]);
                firstName = Convert.ToString(reader["firstName"]);
                lastName = Convert.ToString(reader["lastName"]);
                infix = Convert.ToString(reader["infix"]);
                telNr = Convert.ToString(reader["telNr"]);

                //Store the current right
                currentRights.Add(new Right(Convert.ToInt32(reader["rightId"]), Convert.ToString(reader["rightName"])));
            }

            //Create the final user's role
            Role role = new Role(
                Convert.ToInt32(roleId),
                Convert.ToString(roleName),
                currentRights
            );

            //Add the final user to the list
            userList.Add(new User(
                Convert.ToInt32(userId),
                Convert.ToString(email),
                Convert.ToString(firstName),
                Convert.ToString(lastName),
                Convert.ToString(infix),
                Convert.ToString(telNr),
                role
            ));

            return userList;
        }

        /// <summary>
        /// Update a user's role
        /// </summary>
        /// <param name="user"></param>
        /// <param name="newRoleId"></param>
        /// FUNCTION IS NOT TESTED YET!
        public void UpdateUserRole(User user, Models.Role role)
        {
            string query =
                "UPDATE proftaak.[User] " +
                "SET roleId = @roleId " +
                "WHERE proftaak.[User].email = @email;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@roleId", role.id);
            command.Parameters.AddWithValue("@email", user.Emailaddress.ToString());

            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        /// <summary>
        /// Register a new user with infix
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="infix"></param>
        /// <param name="lastName"></param>
        /// <param name="telnr"></param>
        /// <param name="roleID"></param>
        public void RegisterInfix(string email, string password, string firstName, string infix, string lastName, string telnr)
        {

            string query = "INSERT INTO proftaak.[User](email, password, firstName, lastName, infix, telNr) " +
                           "VALUES (@emailaddress, @password, @firstname, @lastname, @infix, @telNr)";
            SqlCommand command = new SqlCommand(query);
            command.Connection = connection;
            command.Parameters.AddWithValue("@emailaddress", email);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@firstname", firstName);
            command.Parameters.AddWithValue("@lastname", lastName);
            command.Parameters.AddWithValue("@infix", infix);
            command.Parameters.AddWithValue("@telNr", telnr);


            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        /// <summary>
        /// Register a new user without infix
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="telnr"></param>
        /// <param name="roleID"></param>
        public void RegisterNoInfix(string email, string password, string firstName, string lastName, string telnr)
        {

            string query = "INSERT INTO proftaak.[User](email, password, firstName, lastName, telNr) " +
                           "VALUES (@emailaddress, @password, @firstname, @lastname, @telNr)";
            SqlCommand command = new SqlCommand(query);
            command.Connection = connection;
            command.Parameters.AddWithValue("@emailaddress", email);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@firstname", firstName);
            command.Parameters.AddWithValue("@lastname", lastName);
            command.Parameters.AddWithValue("@telNr", telnr);

            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

		/// <summary>
		/// Register a new user
		/// </summary>
		public void Register(string email, string hashedPassword, byte[] salt, string firstName, string lastName, string telNr, string infix)
		{
			SqlCommand command = new SqlCommand();
			command.Connection = connection;

			if(infix != null)
			{
				command.CommandText = "INSERT INTO proftaak.[User] " +
				"(email, password, salt, firstName, lastName, telNr, infix) " +
				"VALUES (@email, @password, @salt, @firstName, @lastName, @telNr, @infix);";

				command.Parameters.Add("@infix", SqlDbType.VarChar);
				command.Parameters["@infix"].Value = infix;
			} else
			{
				command.CommandText = "INSERT INTO proftaak.[User] " +
				"(email, password, salt, firstName, lastName, telNr) " +
				"VALUES (@email, @password, @salt, @firstName, @lastName, @telNr);";
			}

			command.Parameters.Add("@email", SqlDbType.VarChar);
			command.Parameters.Add("@password", SqlDbType.VarChar);
			command.Parameters.Add("@salt", SqlDbType.VarBinary);
			command.Parameters.Add("@firstName", SqlDbType.VarChar);
			command.Parameters.Add("@lastName", SqlDbType.VarChar);
			command.Parameters.Add("@telNr", SqlDbType.VarChar);

			command.Parameters["@email"].Value = email;
			command.Parameters["@password"].Value = hashedPassword;
			command.Parameters["@salt"].Value = salt;
			command.Parameters["@firstName"].Value = firstName;
			command.Parameters["@lastName"].Value = lastName;
			command.Parameters["@telNr"].Value = telNr;

			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
		}
    }
}

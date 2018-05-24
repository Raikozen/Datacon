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
            connection = new SqlConnection("Server=tcp:proftaaks2.database.windows.net,1433;Initial Catalog=Datacon;Persist Security Info=False;User ID=adminuser;Password=StealY0Bike!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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
			command.CommandText = "SELECT salt as salt FROM dbo.[User] WHERE email = @email";

			command.Parameters.Add("@email", SqlDbType.VarChar);
			command.Parameters["@email"].Value = emailAddress;

			connection.Open();
			SqlDataReader reader = command.ExecuteReader();

			byte[] salt;

			if(reader.Read())
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
				"dbo.[User].id as userId, dbo.[User].email as email, dbo.[User].password as password, dbo.[User].firstName as firstName, dbo.[User].lastName as lastName, dbo.[User].infix as infix, dbo.[User].telNr as telNr, dbo.[User].roleId, " +
				"dbo.[Right].id as rightId, dbo.[Right].name as rightName, " +
				"dbo.[Role].name as roleName " +
				"FROM " +
				"dbo.[User] " +
				"INNER JOIN dbo.[Role_Right] on dbo.[Role_Right].roleId = dbo.[User].roleId " +
				"INNER JOIN dbo.[Right] on dbo.[Role_Right].rightId = dbo.[Right].id " +
				"INNER JOIN dbo.[Role] on dbo.[Role_Right].roleId = dbo.[Role].id " +
				"WHERE dbo.[Role_Right].hasRight != 0 AND email = @email AND password LIKE @password " +
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
                "dbo.[User].id as userId, dbo.[User].email as email, dbo.[User].password as password, dbo.[User].firstName as firstName, dbo.[User].lastName as lastName, dbo.[User].infix as infix, dbo.[User].telNr as telNr, dbo.[User].roleId, " +
                "dbo.[Right].id as rightId, dbo.[Right].name as rightName, " +
                "dbo.[Role].name as roleName " +
                "FROM " +
                "dbo.[User] " +
                "INNER JOIN dbo.[Role_Right] on dbo.[Role_Right].roleId = dbo.[User].roleId " +
                "INNER JOIN dbo.[Right] on dbo.[Role_Right].rightId = dbo.[Right].id " +
                "INNER JOIN dbo.[Role] on dbo.[Role_Right].roleId = dbo.[Role].id " +
                "WHERE dbo.[User].id = @userId AND dbo.[Role_Right].hasRight = 1 " +
                "ORDER BY userId";

            command.Parameters.Add("@userId", SqlDbType.Int);

            command.Parameters["@userId"].Value = userId;

            command.Connection.Open();
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
                command.Connection.Close();

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

            command.Connection.Close();
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
                "dbo.[User].id as userId, dbo.[User].email as email, dbo.[User].password as password, dbo.[User].firstName as firstName, dbo.[User].lastName as lastName, dbo.[User].infix as infix, dbo.[User].telNr as telNr, dbo.[User].roleId, " +
                "dbo.[Right].id as rightId, dbo.[Right].name as rightName, " +
                "dbo.[Role].name as roleName " +
                "FROM " +
                "dbo.[User] " +
                "INNER JOIN dbo.[Role_Right] on dbo.[Role_Right].roleId = dbo.[User].roleId " +
                "INNER JOIN dbo.[Right] on dbo.[Role_Right].rightId = dbo.[Right].id " +
                "INNER JOIN dbo.[Role] on dbo.[Role_Right].roleId = dbo.[Role].id " +
                "WHERE dbo.[Role_Right].hasRight != 0 " +
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
        public void UpdateUserRole(User user, Role role)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "UPDATE dbo.[User] " +
				"SET dbo.[User].roleId = @roleId " +
				"WHERE dbo.[User].id = @userid;"; ;

            command.Parameters.Add("@roleId", SqlDbType.Int);
            command.Parameters.Add("@userId", SqlDbType.Int);

            command.Parameters["@roleId"].Value = role.Id;
            command.Parameters["@userId"].Value = user.Id;

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

            string query = "INSERT INTO dbo.[User](email, password, firstName, lastName, infix, telNr) " +
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

            string query = "INSERT INTO dbo.[User](email, password, firstName, lastName, telNr) " +
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
				command.CommandText = "INSERT INTO dbo.[User] " +
				"(email, password, salt, firstName, lastName, telNr, infix) " +
				"VALUES (@email, @password, @salt, @firstName, @lastName, @telNr, @infix);";

				command.Parameters.Add("@infix", SqlDbType.VarChar);
				command.Parameters["@infix"].Value = infix;
			} else
			{
				command.CommandText = "INSERT INTO dbo.[User] " +
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

			command.Connection.Open();
			command.ExecuteNonQuery();
			command.Connection.Close();
		}

        public void ReportSick(int userID)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO dbo.[SickReport] " +
                "(UserId, DateTimeStart) " +
                "VALUES (@userId, @datetimeStart)";
            command.Parameters.AddWithValue("@userId", userID);
            command.Parameters.AddWithValue("@datetimeStart", DateTime.Today);

            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        public bool IsSick(int userID)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT DateTimeEnd FROM dbo.[SickReport] WHERE UserId = @userID";
            command.Parameters.AddWithValue("@userID", userID);
            using (command)
            {
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0))
                        {
                            command.Connection.Close();
                            return true;
                        }
                    }
                    command.Connection.Close();
                    return false;
                }
                else
                {
                    command.Connection.Close();
                    return false;
                }
            }
        }

        public void SicknessRestored(int userID)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "UPDATE dbo.[SickReport] " +
                "SET DateTimeEnd = @datetimeEnd " +
                "WHERE UserId = @userID AND DateTimeEnd IS NULL";
            command.Parameters.AddWithValue("@datetimeEnd", DateTime.Today);
            command.Parameters.AddWithValue("userID", userID);

            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }

        public List<SickReport> GetSickReportsUser(int userID)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT UserId, DateTimeStart, DateTimeEnd, dbo.[User].firstName, " +
                "dbo.[User].infix, dbo.[User].lastName FROM dbo.[SickReport] " +
                "INNER JOIN dbo.[User] ON UserId = dbo.[User].id " +
                "WHERE UserId = @userID";
            command.Parameters.AddWithValue("userID", userID);

            using (command)
            {
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    List<SickReport> sickReports = new List<SickReport>();
                    while (reader.Read())
                    {
                        string fullname = (string)reader["firstName"] + " " + (reader.IsDBNull(4) ? "" : (string)reader["infix"]) + " " + (string)reader["lastName"];
                        sickReports.Add(new SickReport(
                            (int)reader["UserId"],
                            fullname,
                            reader.IsDBNull(1) ? null : (DateTime?)reader["DateTimeStart"],
                            reader.IsDBNull(2) ? null : (DateTime?)reader["DateTimeEnd"]
                        ));
                    }
                    command.Connection.Close();
                    return sickReports;
                }
                else
                {
                    command.Connection.Close();
                    return null;
                }
            }
        }

        public List<SickReport> GetSickReportsAll()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT UserId, DateTimeStart, DateTimeEnd, dbo.[User].firstName, " +
                "dbo.[User].infix, dbo.[User].lastName FROM dbo.[SickReport] " +
                "INNER JOIN dbo.[User] ON UserId = dbo.[User].id ";

            using (command)
            {
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    List<SickReport> sickReports = new List<SickReport>();
                    while (reader.Read())
                    {
                        string fullname = (string)reader["firstName"] + " " + (reader.IsDBNull(4) ? "" : (string)reader["infix"]) + " " + (string)reader["lastName"];
                        sickReports.Add(new SickReport(
                            (int)reader["UserId"],
                            fullname,
                            reader.IsDBNull(1) ? null : (DateTime?)reader["DateTimeStart"],
                            reader.IsDBNull(2) ? null : (DateTime?)reader["DateTimeEnd"]
                        ));
                    }
                    command.Connection.Close();
                    return sickReports;
                }
                else
                {
                    command.Connection.Close();
                    return null;
                }
            }
        }

        public List<HolidayRequest> GetUnapprovedHolidayRequests()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT id, userId, dateStart, dateEnd, description, approved " +
                "FROM dbo.[Holiday] WHERE approved = 0";
            using (command)
            {
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<HolidayRequest> holidayRequests;
                if (reader.HasRows)
                {
                    holidayRequests = new List<HolidayRequest>();
                    while (reader.Read())
                    {
                        holidayRequests.Add(new HolidayRequest(
                            (int)reader["id"], (int)reader["userId"], (DateTime)reader["dateStart"], (DateTime)reader["dateEnd"], 
                            reader.IsDBNull(4) ? "" : (string)reader["description"], (bool)reader["approved"]));
                    }
                }
                else
                {
                    holidayRequests = default;
                }
                command.Connection.Close();
                return holidayRequests;
            }
        }

        public List<HolidayRequest> GetAllHolidayRequests()
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT id, userId, dateStart, dateEnd, description, approved " +
                "FROM dbo.[Holiday]";
            using (command)
            {
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<HolidayRequest> holidayRequests;
                if (reader.HasRows)
                {
                    holidayRequests = new List<HolidayRequest>();
                    while (reader.Read())
                    {
                        holidayRequests.Add(new HolidayRequest(
                            (int)reader["id"], (int)reader["userId"], (DateTime)reader["dateStart"], (DateTime)reader["dateEnd"],
                            reader.IsDBNull(4) ? "" : (string)reader["description"], (bool)reader["approved"]));
                    }
                }
                else
                {
                    holidayRequests = default;
                }
                command.Connection.Close();
                return holidayRequests;
            }
        }

        public void AddHolidayRequest(HolidayRequest holidayRequest)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO dbo.[Holiday] " +
                "(userId, dateStart, dateEnd, description, approved) " +
                "VALUES (@userid, @datestart, @dateend, @description, @approved)";
            command.Parameters.AddWithValue("@userid", holidayRequest.UserId);
            command.Parameters.AddWithValue("@datestart", holidayRequest.DateStart);
            command.Parameters.AddWithValue("@dateend", holidayRequest.DateEnd);
            command.Parameters.AddWithValue("@description", holidayRequest.Description);
            command.Parameters.AddWithValue("@approved", holidayRequest.Approved);

            using (command)
            {
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }

        public void ApproveHolidayRequest(int Id)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "UPDATE dbo.[Holiday] " +
                "SET approved = 1 WHERE id = @id";
            command.Parameters.AddWithValue("@id", Id);

            using (command)
            {
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }

        public List<HolidayRequest> GetUserHolidayRequests(int userId)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT id, userId, dateStart, dateEnd, description, approved " +
                "FROM dbo.[Holiday] WHERE userId = @userID";
            command.Parameters.AddWithValue("@userID", userId);
            using (command)
            {
                command.Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<HolidayRequest> holidayRequests;
                if (reader.HasRows)
                {
                    holidayRequests = new List<HolidayRequest>();
                    while (reader.Read())
                    {
                        holidayRequests.Add(new HolidayRequest(
                            (int)reader["id"], (int)reader["userId"], (DateTime)reader["dateStart"], (DateTime)reader["dateEnd"],
                            reader.IsDBNull(4) ? "" : (string)reader["description"], (bool)reader["approved"]));
                    }
                }
                else
                {
                    holidayRequests = default;
                }
                command.Connection.Close();
                return holidayRequests;
            }
        }

        public void DeleteHolidayRequest(int Id)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM dbo.[Holiday] WHERE id = @id";
            command.Parameters.AddWithValue("@id", Id);

            using (command)
            {
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }

        public void DeleteUser(int userId)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM dbo.[User] WHERE id = @userID";
            command.Parameters.AddWithValue("@userID", userId);
            using (command)
            {
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using App.Datalayer;
using App.Models;

namespace App.Datalayer
{
    public class NewsfeedSQLContext : INewsfeedContext
    {
		private static readonly string connnectionstring = "Server = mssql.fhict.local; Database = dbi338912; User Id = dbi338912; Password = StealYoBike!";

		public List<NewsfeedPost> GetAllNewsfeedPosts()
		{
			List<NewsfeedPost> posts = new List<NewsfeedPost>();

			try
			{
				SqlDataReader myReader = null;
				SqlCommand myCommand = new SqlCommand("SELECT * FROM proftaak.[NewsfeedPost]", new SqlConnection(connnectionstring));
				myCommand.Connection.Open();

				myReader = myCommand.ExecuteReader();

				while (myReader.Read())
				{
					NewsfeedPost post = new NewsfeedPost(
						Convert.ToInt32(myReader["ID"]),
						myReader["Message"].ToString(),
						Convert.ToDateTime(myReader["Date"])
					);
					posts.Add(post);
				}

				myCommand.Connection.Close();
			}
			catch (ArgumentException x)
			{
				Console.WriteLine(x);
			}

			return posts;
		}

		public List<NewsfeedPost> GetAllActiveNewsfeedPosts()
		{
			List<NewsfeedPost> posts = new List<NewsfeedPost>();

			try
			{
				SqlConnection myConnection = new SqlConnection(connnectionstring);
				myConnection.Open();

				SqlDataReader myReader = null;
				SqlCommand myCommand = new SqlCommand(
					"SELECT * FROM proftaak.[NewsfeedPost] " +
					"WHERE [Date] >=dateadd(d,datediff(d,0, getdate()),-3) and [Date]<dateadd(d,datediff(d,0, getdate()),1)", myConnection
				);

				myReader = myCommand.ExecuteReader();

				while (myReader.Read())
				{
					NewsfeedPost post = new NewsfeedPost(
						Convert.ToInt32(myReader["ID"]),
						myReader["Message"].ToString(),
						Convert.ToDateTime(myReader["Date"])
					);
					posts.Add(post);
				}

				myConnection.Close();
			}
			catch (ArgumentException x)
			{
				Console.WriteLine(x);
			}

			return posts;
		}

		public bool CreateNewsfeedPost(string message, DateTime date)
		{
			try
			{
				SqlConnection myConnection = new SqlConnection(connnectionstring);
				myConnection.Open();

				SqlCommand myCommand = new SqlCommand(
					"INSERT INTO proftaak.[NewsfeedPost] " +
					"(Message, Date) " +
					"VALUES (@Message, @Date);", myConnection
				);

				myCommand.Parameters.AddWithValue("@Message", message);
				myCommand.Parameters.AddWithValue("@Date", date);

				myCommand.ExecuteNonQuery();

				myConnection.Close();

				return true;
			}
			catch (ArgumentException x)
			{
				Console.WriteLine(x);
				return false;
			}
		}

		public bool UpdateNewsfeedPost(int id, string message, DateTime date)
		{
			try
			{
				SqlConnection myConnection = new SqlConnection(connnectionstring);
				myConnection.Open();

				SqlCommand myCommand = new SqlCommand(
					"UPDATE OutSource.NewsfeedPost " +
					"SET Message = @Message, Date = @Date " +
					"WHERE ID = @ID;", myConnection
				);

				myCommand.Parameters.AddWithValue("@ID", id);
				myCommand.Parameters.AddWithValue("@Message", message);
				myCommand.Parameters.AddWithValue("@Date", date);

				myCommand.ExecuteNonQuery();

				myConnection.Close();
				return true;
			}
			catch (ArgumentException x)
			{
				Console.WriteLine(x);
				return false;
			}
		}

		public bool DeleteNewsfeedPost(int id)
		{
			try
			{
				SqlConnection myConnection = new SqlConnection(connnectionstring);
				myConnection.Open();

				SqlCommand myCommand = new SqlCommand(
					"DELETE FROM proftaak.[NewsfeedPost] WHERE ID = @ID", myConnection
				);

				myCommand.Parameters.AddWithValue("@ID", id);

				myCommand.ExecuteNonQuery();

				myConnection.Close();
				return true;
			}
			catch (ArgumentException x)
			{
				Console.WriteLine(x);
				return false;
			}
		}

		public void DeleteOldNewsFeedPosts()
		{
			try
			{
				SqlConnection myConnection = new SqlConnection(connnectionstring);			

				SqlCommand myCommand = new SqlCommand();
				myCommand.CommandText =
					"DELETE FROM proftaak.[NewsfeedPost] " +
					"WHERE date < @date";
				myCommand.Connection = myConnection;

				myCommand.Parameters.AddWithValue("@date", DateTime.Now.AddDays(-5).ToString());

				myConnection.Open();
				myCommand.ExecuteNonQuery();
				myConnection.Close();
			}
			catch (ArgumentException x)
			{
				Console.WriteLine(x);
			}
		}
	}
}

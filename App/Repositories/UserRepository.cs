using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using App.Datalayer;
using App.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;

namespace App.Repositorys
{
    public class UserRepository
    {
        public static User loggedinUser = null;
        private IUserContext _Context;

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(IUserContext context)
        {
            this._Context = context;
        }

        /// <summary>
        /// Log a user in
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public User Login(string email, string password)
        {
            loggedinUser = _Context.Login(email, password);

			if(loggedinUser != null)
			{
				return loggedinUser;
			}

			return null;
        }

        /// <summary>
        /// Get a list of all the users
        /// </summary>
        /// <returns></returns>
        public List<User> GetUserList()
        {
            return _Context.GetUserList();
        }

        public void UpdateUserRole(User user, Role role)
        {
            _Context.UpdateUserRole(user, role);
        }

		/// <summary>
		/// Create a new user account
		/// </summary>
		/// <param name="email"></param>
		/// <param name="password"></param>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="telNr"></param>
		/// <param name="infix"></param>
		public void Register(string email, string password, string firstName, string lastName, string telNr, string infix=null)
		{
			//Geneate a salt
			byte[] salt = new byte[128 / 8];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(salt);
			}

			//Hash the password
			string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
			password: password,
			salt: salt,
			prf: KeyDerivationPrf.HMACSHA256,
			iterationCount: 10000,
			numBytesRequested: 256 / 8));

			_Context.Register(email, hashedPassword, salt, firstName, lastName, telNr, infix);
		}

        public void ReportSick(int userID)
        {
            _Context.ReportSick(userID);
        }

        public bool IsSick(int userID)
        {
            return _Context.IsSick(userID);
        }

        public void SicknessRestored(int userID)
        {
            _Context.SicknessRestored(userID);
        }

        public User GetUser(int userID)
        {
            return _Context.GetUser(userID);
        }

        public List<SickReport> GetSickReportsUser(int userID)
        {
            return _Context.GetSickReportsUser(userID);
        }
	}
}

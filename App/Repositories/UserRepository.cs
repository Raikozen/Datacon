using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Datalayer;
using App.Models;

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
        public void Login(string email, string password)
        {
            loggedinUser = _Context.Login(email, password);
        }

        /// <summary>
        /// Get a list of all the users
        /// </summary>
        /// <returns></returns>
        public List<User> GetUserList()
        {
            return _Context.GetUserList();
        }

        public void UpdateUserRole(User user, Models.Role role)
        {
            _Context.UpdateUserRole(user, role);
        }

        public void RegisterwInfix(string email, string password, string firstName, string infix, string lastName, string telnr)
        {
            _Context.RegisterInfix(email, password, firstName, infix, lastName, telnr);
        }

        public void RegisterNoInfix(string email, string password, string firstName, string lastName, string telnr)
        {
            _Context.RegisterNoInfix(email, password, firstName, lastName, telnr);
        }
    }
}

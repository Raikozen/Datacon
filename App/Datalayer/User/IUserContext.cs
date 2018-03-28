using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;

namespace App.Datalayer
{
    public interface IUserContext
    {
        User Login(string email, string password);
        void UpdateUserRole(User user, Models.Role role);
        List<User> GetUserList();
        User GetUser(int userId);

        void RegisterInfix(string email, string password, string firstName, string infix, string lastName, string telnr);
        void RegisterNoInfix(string email, string password, string firstName, string lastName, string telnr);
    }
}

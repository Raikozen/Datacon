using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proftaak_portal.Models;

namespace Proftaak_portal.Datalayer
{
    interface IUserContext
    {
        User Login(string email, string password);
        void UpdateUserRole(User user, Models.Role role);
        List<User> GetUserList();
        User GetUser(int userId);

        void Register(string email, string password, string firstName, string infix, string lastName, string telnr, int roleID);
        
    }
}

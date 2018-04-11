using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class RoleViewModel
    {
        public List<Role> Roles { get; set; }
                
        public List<User> Users { get; set; }

        public int SelectedRoleId { get; set; }

        public int SelectedUserId { get; set; }

        public Role SelectedRole { get; set; }

        public User SelectedUser { get; set; }

                
    }
}

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

        public int selectedRoleId { get; set; }

        public int selectedUserId { get; set; }
    }
}

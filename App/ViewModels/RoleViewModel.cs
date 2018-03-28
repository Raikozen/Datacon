using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class RoleViewModel
    {

        public List<Role> RoleList { get; }

        public string User { get; set; }

        public string Role { get; set; }
    }
}

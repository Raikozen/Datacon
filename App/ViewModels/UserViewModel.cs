using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using App.Models;

namespace App.ViewModels
{
    public class UserViewModel
    {
        public string Emailaddress { get; }
        public string Firstname { get; }
        public string Lastname { get; }
        public string Infix { get; }
        public string Telnr { get; }
        public Role Role { get; }
    }
}

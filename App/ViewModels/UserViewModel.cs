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
        //Email
        [Required(ErrorMessage ="Email is required.")]
        [EmailAddress(ErrorMessage ="Email needs to be a valid Email Adress")]
        public string Email { get; }
        //Password
        [Required(ErrorMessage ="Password is required.")]
        public string Password { get; }
        //First Name
        [Required(ErrorMessage ="First name is required.")]
        public string Firstname { get; }

        [Required(ErrorMessage ="Last name is required.")]
        public string Lastname { get; }

        public string Infix { get; }

        [Required(ErrorMessage ="Telephone number is required.")]
        public string Telnr { get; }

        [Required(ErrorMessage ="Assigning a role is required.")]
        public Role Role { get; }
    }
}

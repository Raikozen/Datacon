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
		[Display(Name = "Email", Prompt = "Email address")]
        public string Email { get; set; }
        //Password
        [Required(ErrorMessage ="Password is required.")]
		[Display(Name = "Password", Prompt = "password")]
		public string Password { get; set; }
        //First Name
        [Required(ErrorMessage ="First name is required.")]
		[Display(Name = "FirstName", Prompt = "First name")]
		public string Firstname { get; set; }

        [Required(ErrorMessage ="Last name is required.")]
		[Display(Name = "LastName", Prompt = "Last name")]
		public string Lastname { get; set; }

		[Display(Name = "infix", Prompt = "Infix")]
		public string Infix { get; set; }

        [Required(ErrorMessage ="Telephone number is required.")]
		[Display(Name = "TelNr", Prompt = "Telephone number")]
		public string Telnr { get; set; }

        [Required(ErrorMessage ="Assigning a roleID is required.")]
        [Display(Name ="RoleID", Prompt ="RoleID")]
        public int RoleID { get; set; }

        //ContactList/DeleteUser
        public List<User> users { get; set; }
        public string sortBy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.ViewModels
{
    public class LoginViewModel
    {
		//Email
		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Email needs to be a valid Email Adress")]
		[Display(Name = "Email", Prompt = "Email address")]
		public string Email { get; set; }
		//Passwordx
		[Required(ErrorMessage = "Password is required.")]
		[Display(Name = "Password", Prompt = "Password")]
		public string Password { get; set; }
	}
}

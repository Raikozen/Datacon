using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class User
    {
        public int Id { get; }
        public string Emailaddress { get; }
        public string Firstname { get; }
        public string Lastname { get; }
        public string Infix { get; }
        public string Telnr { get; }
        public Role Role { get; }

        //Fullname (used for fields that require the full name to be present.)
        public string FullName { get; }

        public User(int id, string emailAddress, string firstName, string lastName, string infix, string telNr, Role role)
        {
            this.Id = id;
            this.Emailaddress = emailAddress;
            this.Firstname = firstName;
            this.Lastname = lastName;
            this.Infix = infix;
            this.Telnr = telNr;
            this.Role = role;

            if (infix != "")
            {
                this.FullName = infix + " " + lastName + " " + firstName;
            }
            else
            {
                this.FullName = lastName + " " + firstName;
            }
        }

		public User()
		{

		}

    }
}

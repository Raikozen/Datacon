using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public class Right
    {
        public int Id { get; }
        public string Name { get; }

		[NotMapped]
		public bool CheckBoxAnswer { get; set; }

        public Right(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class Right
    {
        public int id { get; }
        public string name { get; }

        public Right(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}

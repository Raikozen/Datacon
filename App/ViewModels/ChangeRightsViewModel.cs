using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;

namespace App.ViewModels
{
    public class ChangeRightsViewModel
    {
		public int SelectedRoleId { get; set; } = 0;
		public Role SelectedRole { get; set; } = null;
		public List<Role> Roles { get; set; }
		public List<Right> Rights { get; set; }
        public bool HasRight { get; set; }

		public ChangeRightsViewModel()
		{

		}
    }
}

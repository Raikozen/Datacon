using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Repositorys;
using App.Datalayer;
using App.ViewModels;

namespace App.Controllers
{
    public class RoleController : Controller
    {
        
        [HttpGet]
        public IActionResult Change()
        {
            RoleViewModel roleviewmodel = new RoleViewModel();
            roleviewmodel.Roles = new RoleRepository(new RoleSQLContext()).GetRoles();
            roleviewmodel.Users = new UserRepository(new UserSQLContext()).GetUserList();

            return View("Change", roleviewmodel);
        }

        // insert HttpPost method 
       

		[HttpGet]
		public IActionResult ChangeRights()
		{
			RoleSQLContext contextRole = new RoleSQLContext();
			RoleRepository repoRole = new RoleRepository(contextRole);

			RightSQLContext contextRight = new RightSQLContext();
			RightRepository repoRight = new RightRepository(contextRight);

			List<Role> roles = repoRole.GetRoles();

			ChangeRightsViewModel model = new ChangeRightsViewModel();
			model.Roles = roles;
			model.Rights = repoRight.GetRights();

			model.SelectedRole = roles.First();

			return View("ChangeRights", model);
		}

		[HttpPost]
		public IActionResult ChangeSelectedRole(int selectedRoleId)
		{
			RoleSQLContext contextRole = new RoleSQLContext();
			RoleRepository repoRole = new RoleRepository(contextRole);

			RightSQLContext contextRight = new RightSQLContext();
			RightRepository repoRight = new RightRepository(contextRight);

			List<Role> roleList = repoRole.GetRoles();

			ChangeRightsViewModel model = new ChangeRightsViewModel();
			model.Roles = roleList;
			model.Rights = repoRight.GetRights();

			//Linq query
			var result = from role in roleList
						where role.id == selectedRoleId
						select role;

			//Iterate through Linq query result
			foreach(var role in result)
			{
				model.SelectedRole = role;
			}

			return View("ChangeRights", model);
		}
        
		[HttpPost]
		public IActionResult ChangeRights(ChangeRightsViewModel viewModel)
		{
			return View("ChangeRights");
		}
    }
}

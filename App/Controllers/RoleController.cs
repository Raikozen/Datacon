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

        [HttpPost]
        public IActionResult Change(int user, int role)
        {
            RoleViewModel roleviewmodel = new RoleViewModel();
            roleviewmodel.Roles = new RoleRepository(new RoleSQLContext()).GetRoles();
            roleviewmodel.Users = new UserRepository(new UserSQLContext()).GetUserList();
            UserRepository userRepository = new UserRepository(new UserSQLContext());
            userRepository.UpdateUserRole(roleviewmodel.Users.Find(f=>f.Id==user), roleviewmodel.Roles.Find(kaas=>kaas.id==role));

            roleviewmodel.selectedRoleId = role;
            roleviewmodel.selectedUserId = user;
            
            return View("Change", roleviewmodel);
        }

<<<<<<< HEAD

=======
>>>>>>> cb120202351b5e0e82c248315a32a17122950a47
		/// <summary>
		/// Show the ChangeRights view
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// Change the selected role in the ChangeRights view
		/// </summary>
		/// <param name="selectedRoleId"></param>
		/// <returns></returns>
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
			foreach (var role in result)
			{
				model.SelectedRole = role;
			}

			return View("ChangeRights", model);
		}

		/// <summary>
		/// Update the role's rights
		/// </summary>
		/// <param name="selectedRoleId"></param>
		/// <param name="selectedRights"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult changeRights(int selectedRoleId, List<int> selectedRights)
		{
			RoleSQLContext contextRole = new RoleSQLContext();
			RoleRepository repoRole = new RoleRepository(contextRole);

			if(selectedRights.Count > 0)
			{
				repoRole.UpdateRightsOfRole(selectedRoleId, selectedRights);
			}

			return RedirectToAction("ChangeRights", "Role");
		}
	}
}

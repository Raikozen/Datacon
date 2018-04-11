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
        public IActionResult Change(int selectedUserId, int selectedRoleId)
        {
            RoleViewModel roleviewmodel = new RoleViewModel();
            roleviewmodel.Roles = new RoleRepository(new RoleSQLContext()).GetRoles();
            roleviewmodel.Users = new UserRepository(new UserSQLContext()).GetUserList();
            
            roleviewmodel.selectedUserId = selectedUserId;
            roleviewmodel.selectedRoleId = selectedRoleId;

            Role selectedRole = null;
            
            var result = from user in roleviewmodel.Users
                         where selectedUserId == user.Id
                         select user;

            foreach(var user in result)
            {
                selectedRole = user.Role;
            }

            roleviewmodel.SelectedRole = selectedRole;

            UserRepository userRepository = new UserRepository(new UserSQLContext());
            userRepository.UpdateUserRole(roleviewmodel.Users.Find(f => f.Id == selectedUserId), roleviewmodel.Roles.Find(werknemer => werknemer.Id == selectedRoleId));

            return View("Change", roleviewmodel);
        }

        [HttpPost]
        public IActionResult Watch()
        {
            return null;
        }

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
						 where role.Id == selectedRoleId
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

			if (selectedRights.Count > 0)
			{
				repoRole.UpdateRightsOfRole(selectedRoleId, selectedRights);
			}

			return RedirectToAction("ChangeRights", "Role");
		}
	}
}

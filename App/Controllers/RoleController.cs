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
    public class RoleController : HomeController
    {
        
        [HttpGet]
        public IActionResult Change()
        {
			base.CheckForLogin();

            RoleViewModel roleviewmodel = new RoleViewModel();
            roleviewmodel.Roles = new RoleRepository(new RoleSQLContext()).GetRoles();
            roleviewmodel.Users = new UserRepository(new UserSQLContext()).GetUserList();

            roleviewmodel.SelectedUser = roleviewmodel.Users[0];

            var result = from User in roleviewmodel.Users
                         where roleviewmodel.SelectedUserId == User.Id
                         select User;

            foreach (var User in result)
            {
                roleviewmodel.SelectedRoleId = User.Role.Id;
                roleviewmodel.SelectedUser = User;
            }

            return View("Change", roleviewmodel);
        }

               
        [HttpPost]
        public IActionResult Change(RoleViewModel roleviewmodel)
        {
            roleviewmodel.Roles = new RoleRepository(new RoleSQLContext()).GetRoles();
            roleviewmodel.Users = new UserRepository(new UserSQLContext()).GetUserList();
            
            var result = from User in roleviewmodel.Users
                         where roleviewmodel.SelectedUserId == User.Id
                         select User;

            foreach(var User in result)
            {
                roleviewmodel.SelectedRoleId = User.Role.Id;
                roleviewmodel.SelectedUser = User;
            }


            UserRepository userRepository = new UserRepository(new UserSQLContext());
            userRepository.UpdateUserRole(roleviewmodel.Users.Find(f => f.Id == roleviewmodel.SelectedUserId), roleviewmodel.Roles.Find(f => f.Id == roleviewmodel.SelectedRoleId));

            return View("Change", roleviewmodel);
        }






		/// <summary>
		/// Show the ChangeRights view
		/// </summary>
		/// <returns></returns>
        [HttpPost]
        public IActionResult Watch()
        {
            return null;
        }

		[HttpGet]
		public IActionResult ChangeRights()
		{
			base.CheckForLogin();

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
			base.CheckForLogin();

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
		public IActionResult ChangeRights(int selectedRoleId, List<int> selectedRights)
		{
			base.CheckForLogin();

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

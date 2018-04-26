﻿using System;
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

            if (!base.CheckForRight(8))
            {
                return RedirectToAction("Index", "Home");
            }

            RoleViewModel viewModel = new RoleViewModel();

            viewModel.Roles = new RoleRepository(new RoleSQLContext()).GetRoles();
            viewModel.Users = new UserRepository(new UserSQLContext()).GetUserList();

            viewModel.SelectedUser = viewModel.Users.Find(u => u.Id == Convert.ToInt32(Request.Cookies["userId"]));
            viewModel.SelectedRole = viewModel.SelectedUser.Role;

            return View("Change", viewModel);

        }


		[HttpPost]
        public IActionResult Change(RoleViewModel viewModel)
        {
            viewModel.Roles = new RoleRepository(new RoleSQLContext()).GetRoles();
            viewModel.Users = new UserRepository(new UserSQLContext()).GetUserList();

			int selectedUserId = viewModel.SelectedUserId;
			int selectedRoleId = viewModel.SelectedRoleId;

			viewModel.SelectedUser = viewModel.Users.Find(x => x.Id == selectedUserId);
			viewModel.SelectedRole = viewModel.Roles.Find(x => x.Id == selectedRoleId);

			UserRepository userRepository = new UserRepository(new UserSQLContext());
            userRepository.UpdateUserRole(viewModel.SelectedUser, viewModel.SelectedRole);

            //Confirmation message apply / create other methods in other controllers (Tim)
            ConfirmChange();

            return View("Change", viewModel);
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
            if (!base.CheckForRight(9))
            {
                return RedirectToAction("Index", "Home");
            }

            RoleSQLContext contextRole = new RoleSQLContext();
            RoleRepository repoRole = new RoleRepository(contextRole);

            RightSQLContext contextRight = new RightSQLContext();
            RightRepository repoRight = new RightRepository(contextRight);

            List<Role> roles = repoRole.GetRoles();

            ChangeRightsViewModel model = new ChangeRightsViewModel();
            model.Roles = roles;
            model.Rights = repoRight.GetRights();

            model.SelectedRole = model.Roles[0];

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

        //Confirmation message apply / create other methods in other controllers (Tim)
        private void ConfirmChange()
        {
            ViewData["message"] = "The user role has been updated successfully";
            
        }
    }
}

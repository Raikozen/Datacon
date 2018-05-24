using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using App.Models;
using App.Repositorys;
using App.Datalayer;
using App.ViewModels;

namespace App.Controllers
{
    public class RoleController : HomeController
    {
        [HttpGet]
        public IActionResult ChangeRoleAndRights()
        {
            base.CheckForLogin();

            if (!base.CheckForRight(5) && !base.CheckForRight(6))
            {
                return RedirectToAction("Index", "Home");
            }

            RoleRepository repoRole = new RoleRepository(new RoleSQLContext());
            RightRepository repoRight = new RightRepository(new RightSQLContext());
            UserRepository repoUser = new UserRepository(new UserSQLContext());

            RoleViewModel RoleviewModel = new RoleViewModel();
            if (base.CheckForRight(5))
            {
                RoleviewModel.HasRight = true;
            }
            RoleviewModel.Roles = repoRole.GetRoles();
            RoleviewModel.Users = repoUser.GetUserList();
            RoleviewModel.SelectedUser = HttpContext.Session.GetInt32("id") == 5 ? RoleviewModel.Users.Find(u=>u.Id != 5) : RoleviewModel.Users.Find(u => u.Id == Convert.ToInt32(HttpContext.Session.GetInt32("id")));
            RoleviewModel.SelectedRole = RoleviewModel.SelectedUser.Role;

            ChangeRightsViewModel RightsviewModel = new ChangeRightsViewModel();
            if (base.CheckForRight(6))
            {
                RightsviewModel.HasRight = true;
            }
            RightsviewModel.Roles = repoRole.GetRoles();
            RightsviewModel.Rights = repoRight.GetRights();
            RightsviewModel.SelectedRole = RightsviewModel.Roles.Find(f => f.Id != 1);

            return View("ChangeRoleAndRights", Tuple.Create(RoleviewModel, RightsviewModel));
        }

        [HttpPost]
        public IActionResult ChangeRole(RoleViewModel RoleviewModel)
        {
            base.CheckForLogin();

            if (!base.CheckForRight(5))
            {
                return RedirectToAction("Index", "Home");
            }

            RoleRepository repoRole = new RoleRepository(new RoleSQLContext());
            RightRepository repoRight = new RightRepository(new RightSQLContext());
            UserRepository repoUser = new UserRepository(new UserSQLContext());

            if (base.CheckForRight(5))
            {
                RoleviewModel.HasRight = true;
            }

            RoleviewModel.Roles = repoRole.GetRoles();
            RoleviewModel.Users = repoUser.GetUserList();

            int selectedUserId = RoleviewModel.SelectedUserId;
            int selectedRoleId = RoleviewModel.SelectedRoleId;

            RoleviewModel.SelectedUser = RoleviewModel.Users.Find(x => x.Id == selectedUserId);
            RoleviewModel.SelectedRole = RoleviewModel.Roles.Find(x => x.Id == selectedRoleId);

            //Don't change the application's default admin
            if (RoleviewModel.SelectedUser.Id != 5)
            {
                UserRepository userRepository = new UserRepository(new UserSQLContext());
                userRepository.UpdateUserRole(RoleviewModel.SelectedUser, RoleviewModel.SelectedRole);

                //Confirmation message apply / create other methods in other controllers (Tim)
                ConfirmChange(RoleviewModel);
            }
            else
            {
                ErrorRole();
            }

            ChangeRightsViewModel RightsviewModel = new ChangeRightsViewModel();
            if (base.CheckForRight(6))
            {
                RightsviewModel.HasRight = true;
            }
            RightsviewModel.Roles = repoRole.GetRoles();
            RightsviewModel.Rights = repoRight.GetRights();
            RightsviewModel.SelectedRole = RightsviewModel.Roles.Find(f => f.Id != 1);

            return View("ChangeRoleAndRights", Tuple.Create(RoleviewModel, RightsviewModel));
        }

        [HttpPost]
        public IActionResult ChangeSelectedRole(int selectedRoleId)
        {
            base.CheckForLogin();

            if (!base.CheckForRight(6))
            {
                return RedirectToAction("Index", "Home");
            }

            RoleRepository repoRole = new RoleRepository(new RoleSQLContext());
            RightRepository repoRight = new RightRepository(new RightSQLContext());
            UserRepository repoUser = new UserRepository(new UserSQLContext());

            RoleViewModel RoleviewModel = new RoleViewModel();
            if (base.CheckForRight(5))
            {
                RoleviewModel.HasRight = true;
            }
            RoleviewModel.Roles = repoRole.GetRoles();
            RoleviewModel.Users = repoUser.GetUserList();
            RoleviewModel.SelectedUser = RoleviewModel.Users.Find(u => u.Id == Convert.ToInt32(HttpContext.Session.GetInt32("id")));
            RoleviewModel.SelectedRole = RoleviewModel.SelectedUser.Role;

            List<Role> roleList = repoRole.GetRoles();

            ChangeRightsViewModel RightsviewModel = new ChangeRightsViewModel();
            if (base.CheckForRight(6))
            {
                RightsviewModel.HasRight = true;
            }
            RightsviewModel.Roles = roleList;
            RightsviewModel.Rights = repoRight.GetRights();

            //Linq query
            var result = from role in roleList
                         where role.Id == selectedRoleId
                         select role;

            //Iterate through Linq query result
            foreach (var role in result)
            {
                RightsviewModel.SelectedRole = role;
            }

            return View("ChangeRoleAndRights", Tuple.Create(RoleviewModel, RightsviewModel));
        }

        [HttpPost]
        public IActionResult ChangeRights(int selectedRoleId, List<int> selectedRights)
        {
            base.CheckForLogin();

            if (!base.CheckForRight(6))
            {
                return RedirectToAction("Index", "Home");
            }

            RoleRepository repoRole = new RoleRepository(new RoleSQLContext());
            RightRepository repoRight = new RightRepository(new RightSQLContext());
            UserRepository repoUser = new UserRepository(new UserSQLContext());

            if (selectedRoleId != 1)
			{
				repoRole.UpdateRightsOfRole(selectedRoleId, selectedRights);

            } else
			{
				ErrorRights();
			}

            ConfirmUpdateRights();

            return RedirectToAction("ChangeRoleAndRights");
        }

        //Confirmation message apply / create other methods in other controllers (Tim)
        private void ConfirmChange(RoleViewModel viewModel)
        {
            ViewData["ConfirmChange"] = "The role for the user '" + viewModel.SelectedUser.Firstname + "' has been updated successfully.";
            
        }

        private void ConfirmUpdateRights()
        {
            TempData["ConfirmUpdateRights"] = "The rights for the selected role have been updated successfully.";
        }

		private void ErrorRole()
		{
			ViewData["ErrorRole"] = "The application admin cannot be updated.";
		}

        private void ErrorRights()
        {
            ViewData["ErrorRights"] = "The default admin user role cannnot be updated";
        }
    }
}

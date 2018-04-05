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
            userRepository.UpdateUserRole(roleviewmodel.Users.Find(f => f.Id == selectedUserId), roleviewmodel.Roles.Find(kaas => kaas.Id == selectedRoleId));

            return View("Change", roleviewmodel);
        }

        [HttpPost]
        public IActionResult Watch()
        {
            return null;
        }

    }
}

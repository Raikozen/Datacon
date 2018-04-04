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

        [HttpPost]
        public IActionResult Watch()
        {
            return null;
        }

    }
}

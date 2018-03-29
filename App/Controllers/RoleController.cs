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
       


        
    }
}

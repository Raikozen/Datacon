using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Repositorys;
using App.Datalayer;

namespace App.Controllers
{
    public class RoleController : Controller
    {
        
        [HttpGet]
        public IActionResult Change()
        {
            ViewData["Message"] = "Your changerole page.";
            return View();
        }

        public IActionResult GetRoles()
        {
            RoleRepository roleRep = new RoleRepository(new RoleSQLContext());
            List<Role> roles = roleRep.GetRoles();
            return View("Change", roles);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;

namespace App.Datalayer
{
    public interface IRoleContext
    {
        void UpdateRoleRights(int roleID, List<int> rightIDs);
        List<Role> GetRoles();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Datalayer
{
    public interface IRoleContext
    {
        void UpdateRoleRights(int roleID, List<int> rightIDs);
        List<Models.Role> GetRoles();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Datalayer;

namespace App.Repositorys
{
    public class RoleRepository
    {
        private IRoleContext _Context;

        public RoleRepository(IRoleContext context)
        {
            this._Context = context;
        }

        public void UpdateRightsOfRole(int roleID, List<int> rightIDs)
        {
            _Context.UpdateRoleRights(roleID, rightIDs);
        }

        public List<Models.Role> GetRoles()
        {
            return _Context.GetRoles();
        }
    }
}

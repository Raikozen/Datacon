using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Datalayer;
using App.Repositorys;

namespace App.Models
{
    public class Role
    {
        private List<Right> rights;

        public int Id { get; }
        public string Name { get; }
        public List<Right> Rights
        {
            get
            {
                return this.rights;
            }
        }

        public Role(int id, string name, List<Right> rights)
        {
            this.Id = id;
            this.Name = name;
            this.rights = rights;
        }

        /// <summary>
        /// Function to update (change) all rights of the role
        /// </summary>
        public void UpdateRights(List<Right> rights)
        {
            this.rights = rights;

            List<int> IDrights = new List<int>();
            foreach (Right right in rights)
            {
                IDrights.Add(right.Id);
            }
            RoleSQLContext roleSQLContext = new RoleSQLContext();
            RoleRepository roleRepository = new RoleRepository(roleSQLContext);
            roleRepository.UpdateRightsOfRole(this.Id, IDrights);
        }
    }
}

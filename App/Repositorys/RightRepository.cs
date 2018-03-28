using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proftaak_portal.Datalayer;
using Proftaak_portal.Models;

namespace Proftaak_portal.Repositorys
{
    class RightRepository
    {
        private IRightContext _Context;

        public RightRepository(IRightContext context)
        {
            this._Context = context;
        }
        public List<Right> GetRights()
        {
            return _Context.GetRights();
        }
    }
}

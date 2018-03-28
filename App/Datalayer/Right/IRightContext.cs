using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proftaak_portal.Models;

namespace Proftaak_portal.Datalayer
{
    interface IRightContext
    {
        List<Models.Right> GetRights();
    }
}

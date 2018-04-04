using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;

namespace App.Datalayer
{
    interface IRightContext
    {
        List<Right> GetRights();
    }
}

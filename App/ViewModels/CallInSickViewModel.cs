using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;
using App.Repositorys;
using App.Datalayer;

namespace App.ViewModels
{
    public class CallInSickViewModel
    {
        public bool isSick;
        public bool hasOverviewRight;
        public List<SickReport> SickReportsUser;
        public List<SickReport> SickReportsAll;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models;

namespace App.ViewModels
{
    public class HolidaysViewModel
    {
        public bool HasApproveHolidayRight;
        private List<HolidayRequest> allholidayRequests;
        private List<HolidayRequest> unapprovedholidayRequests;
        public List<HolidayRequest> UserholidayRequests;

        public HolidaysViewModel(bool hasApproveHolidayRight, List<HolidayRequest> allholidayRequests, List<HolidayRequest> unapprovedholidayRequests, List<HolidayRequest> userholidayRequests)
        {
            this.HasApproveHolidayRight = hasApproveHolidayRight;
            this.allholidayRequests = allholidayRequests;
            this.unapprovedholidayRequests = unapprovedholidayRequests;
            this.UserholidayRequests = userholidayRequests;
        }

        public List<HolidayRequest> AllholidayRequests
        {
            get
            {
                if (HasApproveHolidayRight)
                {
                    return allholidayRequests;
                }
                else
                {
                    return default;
                }
            }
        }

        public List<HolidayRequest> UnapprovedholidayRequests
        {
            get
            {
                if (HasApproveHolidayRight)
                {
                    return unapprovedholidayRequests;
                }
                else
                {
                    return default;
                }
            }
        }
    }
}

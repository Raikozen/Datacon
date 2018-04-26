using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        //DateStart
        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "dateStart")]
        public DateTime? DateStart { get; set; }
        //DateEnd
        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "dateEnd")]
        public DateTime? DateEnd { get; set; }
        //Description
        [Required(ErrorMessage = "Holiday description is required")]
        [Display(Name = "description")]
        public string Description { get; set; }

        public HolidaysViewModel(bool hasApproveHolidayRight, List<HolidayRequest> allholidayRequests, List<HolidayRequest> unapprovedholidayRequests, List<HolidayRequest> userholidayRequests)
        {
            this.HasApproveHolidayRight = hasApproveHolidayRight;
            this.allholidayRequests = allholidayRequests;
            this.unapprovedholidayRequests = unapprovedholidayRequests;
            this.UserholidayRequests = userholidayRequests;
        }

        public HolidaysViewModel()
        {

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
            set { allholidayRequests = value; }
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
            set { unapprovedholidayRequests = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class SickReport
    {
        public int UserID { get; }
        public DateTime? DatetimeStart { get; }
        public DateTime? DatetimeEnd { get; }

        public SickReport(int UserID, DateTime? DatetimeStart, DateTime? DatetimeEnd)
        {
            this.UserID = UserID;
            this.DatetimeStart = DatetimeStart;
            this.DatetimeEnd = DatetimeEnd;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class SickReport
    {
        public int UserID { get; }
        public string UserName { get; }
        private DateTime? datetimeStart { get; }
        private DateTime? datetimeEnd { get; }

        public SickReport(int UserID, string UserName, DateTime? DatetimeStart, DateTime? DatetimeEnd)
        {
            this.UserID = UserID;
            this.UserName = UserName;
            this.datetimeStart = DatetimeStart;
            this.datetimeEnd = DatetimeEnd;
        }

        public string DatetimeStartstring
        {
            get { return datetimeStart == null ? null : datetimeStart.Value.ToShortDateString(); }
        }

        public string DatetimeEndstring
        {
            get { return datetimeEnd == null ? null : datetimeEnd.Value.ToShortDateString(); }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Models
{
   public class DayReportTelePhoneMsg 
    {
       
        public int Id { get; set; }
     
        public TimeSpan? Time { get; set; }

        public string Caller { get; set; }

        public string MessageFor { get; set; }

        public string Message { get; set; }

        public string Signature { get; set; }
        public string TimeString { get; set; }

        public int ShiftId { get; set; }
    }
}

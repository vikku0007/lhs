using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Models
{
    public class DayReportSupportWorkers 
    {
      
        public int Id { get; set; }
        public int? StaffName { get; set; }

        public TimeSpan? TimeIn { get; set; }

        public TimeSpan? TimeOut { get; set; }

        public string Signature { get; set; }

        public int ShiftId { get; set; }
        public string TimeInString { get; set; }

        public string TimeOutString { get; set; }
    }
}

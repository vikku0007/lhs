using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Models
{
  public class DayReportVisitor 
    {
        
        public int Id { get; set; }
       
        public TimeSpan? Time { get; set; }
        public string TimeString { get; set; }

        public string VisitorName { get; set; }

        public string VisitReason { get; set; }

        public int ShiftId { get; set; }
    }
}

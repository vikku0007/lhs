using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
   public class EmployeeLeaveModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        // public bool Approve { get; set; }
        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
        public int LeaveType { get; set; }
        public string ReasonOfLeave { get; set; }
        public string LeaveTypeName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsRejected { get; set; }
    }
}

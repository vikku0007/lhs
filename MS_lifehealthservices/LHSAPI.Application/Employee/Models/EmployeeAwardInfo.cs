using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
    public class EmployeeAwardInfo
    {
        public int Id { get; set; }
        public int AwardGroup { get; set; }
        public string AwardGroupName { get; set; }

        public int EmployeeId { get; set; }

        public double? Allowances { get; set; }

        public double? Dailyhours { get; set; }

        public double? Weeklyhours { get; set; }


    }
}

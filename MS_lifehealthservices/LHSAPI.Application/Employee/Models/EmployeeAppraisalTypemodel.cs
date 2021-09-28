using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
     public class EmployeeAppraisalTypemodel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string DepartmentName { get; set; }

        public int AppraisalType { get; set; }

        public DateTime? AppraisalDateFrom { get; set; }

        public DateTime? AppraisalDateTo { get; set; }
       
        public string AppraisalTypeName { get; set; }
        public DateTime? CreatedDate { get; set; }
        
    }
}

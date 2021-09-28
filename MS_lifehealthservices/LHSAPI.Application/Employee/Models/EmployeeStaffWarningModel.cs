using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Models
{
    public class EmployeeStaffWarningModel
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DateTime? Date { get; set; }

        public string JobTitle { get; set; }

        public string ManagerName { get; set; }

        public string DepartmentName { get; set; }

        public int WarningType { get; set; }

        public int OffensesType { get; set; }

        public string Description { get; set; }

        public string ImprovementPlan { get; set; }

        public string Warning { get; set; }
        public string OtherOffenses { get; set; }

        public string OffensesTypeName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

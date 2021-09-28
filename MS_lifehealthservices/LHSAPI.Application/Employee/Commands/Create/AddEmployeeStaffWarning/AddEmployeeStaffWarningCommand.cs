using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeStaffWarning
{
  public class AddEmployeeStaffWarningCommand : IRequest<ApiResponse>
  {
        public int EmployeeId { get; set; }

        public DateTime? Date { get; set; }

        public string JobTitle { get; set; }

        public string ManagerName { get; set; }

        public string DepartmentName { get; set; }

        public int WarningType { get; set; }

        public int OffensesType { get; set; }

        public string Description { get; set; }

        public string ImprovementPlan { get; set; }
        public string OtherOffenses { get; set; }

    }
}

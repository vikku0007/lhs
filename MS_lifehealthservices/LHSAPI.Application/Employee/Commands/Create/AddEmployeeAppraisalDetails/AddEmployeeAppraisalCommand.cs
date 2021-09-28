using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeAppraisalDetails
{
  public class AddEmployeeAppraisalDetailsCommand : IRequest<ApiResponse>
  {
        public int EmployeeId { get; set; }

      //  public string DepartmentName { get; set; }

        public int AppraisalType { get; set; }

        public DateTime? AppraisalDateFrom { get; set; }

        public DateTime? AppraisalDateTo { get; set; }
        public List<ApprisalStandardInfo> StandardList { get; set; }
    }
    public class ApprisalStandardInfo
    {
        public int EmployeeId { get; set; }

        public bool IsExceeds { get; set; }

        public bool IsAchieves { get; set; }

        public bool IsBelow { get; set; }

        public string DescriptionName { get; set; }
    }
}

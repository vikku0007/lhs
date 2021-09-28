using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Update.EditEmployeeAppraisalInfo
{
    public class EditEmployeeAppraisalInfoCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
       // public string DepartmentName { get; set; }

        public int AppraisalType { get; set; }

        public DateTime? AppraisalDateFrom { get; set; }

        public DateTime? AppraisalDateTo { get; set; }
        public List<EditApprisalStandardInfo> StandardList { get; set; }
    }
    public class EditApprisalStandardInfo
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public bool IsExceeds { get; set; }

        public bool IsAchieves { get; set; }

        public bool IsBelow { get; set; }

        public string DescriptionName { get; set; }
    }
}

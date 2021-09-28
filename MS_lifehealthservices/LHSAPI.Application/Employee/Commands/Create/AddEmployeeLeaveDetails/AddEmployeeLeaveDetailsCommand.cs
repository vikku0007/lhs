using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeLeaveDetails
{
  public class AddEmployeeLeaveDetailsCommand : IRequest<ApiResponse>
  {
        public int EmployeeId { get; set; }

        public int LeaveType { get; set; }
        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
        public string ReasonOfLeave { get; set; }

    }
}

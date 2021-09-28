using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeCommunicationInfo
{
  public class AddEmployeeCommunicationInfoCommand : IRequest<ApiResponse>
  {
        public int EmployeeId { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }
        public int[] AssignedTo { get; set; }
        

    }
}

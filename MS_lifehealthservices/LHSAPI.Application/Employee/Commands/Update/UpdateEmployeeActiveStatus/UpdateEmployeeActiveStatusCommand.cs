using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Update.UpdateEmployeeActiveStatus
{
  public class UpdateEmployeeActiveStatusCommand : IRequest<ApiResponse>
  {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public bool Status { get; set; }
        


    }
}

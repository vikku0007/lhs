using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Update.EditEmployeeCommunication
{
  public class EditEmployeeCommunicationCommand : IRequest<ApiResponse>
  {
        public int Id { get; set; }
        public int[] EmployeeId { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public int AssignedTo { get; set; }


    }
}

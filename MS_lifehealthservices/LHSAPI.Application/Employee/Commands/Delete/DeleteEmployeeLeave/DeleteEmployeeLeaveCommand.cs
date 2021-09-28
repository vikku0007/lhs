using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeLeave
{
    public class DeleteEmployeeLeaveCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}

using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Update.ApproveEmployeeLeave
{
    public class RejectEmployeeLeaveInfoCommand : IRequest<ApiResponse>
    {
        public int? Id { get; set; }
        public string Remark { get; set; }
    }
}

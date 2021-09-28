using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateLeaveApproveStatus
{
    public class UpdateLeaveApproveStatusCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int EmpId { get; set; }
    }
}

using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateLeaveRejectStatus
{
    public class UpdateLeaveRejectStatusCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string RejectRemark { get; set; }
        public int EmpId { get; set; }
    }
}

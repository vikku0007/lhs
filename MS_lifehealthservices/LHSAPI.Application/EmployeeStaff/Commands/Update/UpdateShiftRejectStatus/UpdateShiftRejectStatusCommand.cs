using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateShiftRejectStatus
{
    public class UpdateShiftRejectStatusCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public string Remark { get; set; }
    }
}

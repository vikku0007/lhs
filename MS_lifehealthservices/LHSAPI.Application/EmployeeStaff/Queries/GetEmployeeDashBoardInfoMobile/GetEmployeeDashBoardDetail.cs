
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeDashBoardInfoMobile
{
    public class GetEmployeeDashBoardDetail : IRequest<ApiResponse>
    {
        public int EmployeeId { get; set; }
    }
}

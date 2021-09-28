
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Queries.GetEmployeeTodayShiftInfo
{
    public class GetEmployeeTodayShiftInfoQuery : IRequest<ApiResponse>
    {

        public int EmployeeId { get; set; }


    }
}

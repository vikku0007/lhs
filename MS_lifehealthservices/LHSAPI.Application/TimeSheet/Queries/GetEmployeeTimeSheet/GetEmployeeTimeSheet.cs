using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.TimeSheet.Queries.GetEmployeeTimeSheet
{
    public class GetEmployeeTimeSheet : IRequest<ApiResponse>
    {
        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}


using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetReportedByEmployee
{
    public class GetReportedByEmployeeQuery : IRequest<ApiResponse>
    {
      
        public int EmployeeId { get; set; }

    }
}

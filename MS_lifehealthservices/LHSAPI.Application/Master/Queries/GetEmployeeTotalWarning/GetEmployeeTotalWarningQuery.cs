
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetEmployeeTotalWarning
{
    public class GetEmployeeTotalWarningQuery : IRequest<ApiResponse>
    {
      
        public int EmployeeId { get; set; }

    }
}

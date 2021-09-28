
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetEmployeeLevel
{
    public class GetEmployeeLevelQuery : IRequest<ApiResponse>
    {
      
        public int EmployeeId { get; set; }
    }
}

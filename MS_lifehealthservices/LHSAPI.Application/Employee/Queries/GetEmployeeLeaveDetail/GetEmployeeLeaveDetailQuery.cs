
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Queries.GetEmployeeLeaveDetail
{
    public class GetEmployeeLeaveDetailQuery : IRequest<ApiResponse>
    {


        public int Id { get; set; }

    }
}

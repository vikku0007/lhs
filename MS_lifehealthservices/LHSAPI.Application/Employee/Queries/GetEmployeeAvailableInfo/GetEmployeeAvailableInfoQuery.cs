
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Queries.GetEmployeeAvailableInfo
{
    public class GetEmployeeAvailableInfoQuery : IRequest<ApiResponse>
    {


        public int Id { get; set; }

    }
}

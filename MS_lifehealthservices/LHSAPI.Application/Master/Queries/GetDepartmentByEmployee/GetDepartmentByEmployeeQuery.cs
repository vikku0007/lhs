
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetDepartmentByEmployee
{
    public class GetDepartmentByEmployeeQuery : IRequest<ApiResponse>
    {
        public int EmployeeId { get; set; }
        public string CodeData { get; set; }

        public string CodeDescription { get; set; }
    }
}

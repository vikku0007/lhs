using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeAwardInfo
{
    public class AddEmployeeAwardInfoCommand : IRequest<ApiResponse>
    {
        public int EmployeeId { get; set; }
        public int AwardGroup { get; set; }

        public double? Allowances { get; set; }

        public double? Dailyhours { get; set; }

        public double? Weeklyhours { get; set; }

    }
}

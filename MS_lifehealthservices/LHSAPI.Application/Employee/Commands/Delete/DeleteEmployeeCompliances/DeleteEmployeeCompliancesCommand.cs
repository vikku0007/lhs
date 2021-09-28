using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeCompliances
{
    public class DeleteEmployeeCompliancesCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}

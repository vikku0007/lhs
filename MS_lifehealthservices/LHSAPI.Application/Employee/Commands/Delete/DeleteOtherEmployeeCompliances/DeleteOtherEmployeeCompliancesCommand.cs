using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteOtherEmployeeCompliances
{
    public class DeleteOtherEmployeeCompliancesCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}

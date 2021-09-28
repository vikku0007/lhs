using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeDrivingLicense
{
    public class DeleteEmployeeDrivingLicenseCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}

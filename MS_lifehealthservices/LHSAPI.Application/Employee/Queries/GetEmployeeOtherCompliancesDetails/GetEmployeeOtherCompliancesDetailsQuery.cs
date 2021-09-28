
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Queries.GetEmployeeOtherCompliancesDetails
{
    public class GetEmployeeOtherCompliancesDetailsQuery : IRequest<ApiResponse>
    {

        public int Id { get; set; }

    }
}

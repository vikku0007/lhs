
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Queries.GetEmployeeOtherComplianceList
{
    public class GetEmployeeOtherComplianceListQuery : IRequest<ApiResponse>
    {
        public int EmployeeId { get; set; }
       
        public int PageSize{get;set;}
        
        public int PageNo{get;set;}

  }
}

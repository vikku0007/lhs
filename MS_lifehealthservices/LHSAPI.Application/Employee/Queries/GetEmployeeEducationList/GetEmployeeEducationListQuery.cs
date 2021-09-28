
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Queries.GetEmployeeEducationList
{
    public class GetEmployeeEducationListQuery : IRequest<ApiResponse>
    {
    public int EmployeeId { get; set; }
   
    public int PageSize{get;set;}

    public int PageNo{get;set;}
    public LHSAPI.Common.Enums.Employee.EmployeeEducationOrderBy OrderBy { get; set; }
    public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }

    }
}

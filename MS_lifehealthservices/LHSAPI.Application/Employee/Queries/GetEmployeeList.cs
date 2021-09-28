
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Queries
{
    public class GetEmployeeList : IRequest<ApiResponse>
    {
   

    public string SearchTextByName{get;set;}

    public string SearchTextByLocation{get;set;}

    public int PageSize{get;set;}

    public int PageNo{get;set;}

    public LHSAPI.Common.Enums.Employee.EmployeeOrderBy OrderBy { get; set; }
    public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }

  }
}

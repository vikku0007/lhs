
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeApplyLeave
{
    public class GetEmployeeApplyLeaveQuery : IRequest<ApiResponse>
    {
      
        public int EmployeeId { get; set; }
        public int PageSize { get; set; }

        public int PageNo { get; set; }
        public LHSAPI.Common.Enums.Employee.ApplyLeaveInfoOrderBy OrderBy { get; set; }
        public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }
    }
}

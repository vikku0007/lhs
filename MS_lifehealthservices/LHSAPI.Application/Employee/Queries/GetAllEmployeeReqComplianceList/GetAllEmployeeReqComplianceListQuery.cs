
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Queries.GetAllEmployeeReqComplianceList
{
    public class GetAllEmployeeReqComplianceListQuery : IRequest<ApiResponse>
    {
        public string SearchTextByName { get; set; }

        public string SearchTextBydoc { get; set; }
        public int PageSize { get; set; }

        public int PageNo { get; set; }
        public LHSAPI.Common.Enums.Employee.EmployeeComplianceOrderBy OrderBy { get; set; }
        public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }

    }
}

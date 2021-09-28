
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Queries.GetEmployeeCommunicationList
{
    public class GetEmployeeCommunicationListQuery : IRequest<ApiResponse>
    {
        public int EmployeeId { get; set; }

        public string SearchTextBySubject { get; set; }

        public string SearchTextByAssignedTo { get; set; }

        public int PageSize { get; set; }

        public int PageNo { get; set; }
        public LHSAPI.Common.Enums.Employee.EmployeeCommunicationInfoOrderBy OrderBy { get; set; }
        public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }


    }
}

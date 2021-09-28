
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Queries.GetAllEmployeeCommunicationList
{
    public class GetAllEmployeeCommunicationListQuery : IRequest<ApiResponse>
    {

        public string SearchTextByName { get; set; }

        public string SearchTextBySubject { get; set; }
        public int PageSize { get; set; }

        public int PageNo { get; set; }

        public LHSAPI.Common.Enums.Employee.EmployeeCommunicationOrderBy OrderBy { get; set; }
        public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }

    }
}

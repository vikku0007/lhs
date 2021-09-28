
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Administration.Queries.GetUserActivityLog
{
    public class GetUserActivityLogQuery : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public string SearchTextByName { get; set; }

        public int PageSize { get; set; }

        public int PageNo { get; set; }
        public LHSAPI.Common.Enums.Employee.ActivityLogOrderBy OrderBy { get; set; }
        public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }

    }
}

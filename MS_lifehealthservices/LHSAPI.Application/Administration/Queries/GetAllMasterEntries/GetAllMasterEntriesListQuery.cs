
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Administration.Queries.GetAllMasterEntries
{
    public class GetAllMasterEntriesListQuery : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public string searchTextByCodeData { get; set; }

        public string searchTextByCodeDescription { get; set; }
        public int PageSize { get; set; }

        public int PageNo { get; set; }
        public LHSAPI.Common.Enums.Employee.StandardCodeOrderBy OrderBy { get; set; }
        public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }

    }
}

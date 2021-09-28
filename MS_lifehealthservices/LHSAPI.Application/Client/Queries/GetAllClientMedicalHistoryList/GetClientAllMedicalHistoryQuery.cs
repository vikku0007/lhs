
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetAllClientMedicalHistoryList
{
    public class GetClientAllMedicalHistoryQuery : IRequest<ApiResponse>
    {
        public string SearchTextByName { get; set; }

        public string SearchTextByGender { get; set; }
        public int PageSize { get; set; }

        public int PageNo { get; set; }
        public LHSAPI.Common.Enums.Client.ClientMedicalOrderBy OrderBy { get; set; }
        public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }

    }
}

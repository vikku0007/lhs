
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetClientFundingTypeList
{
    public class GetClientFundingTypeListQuery : IRequest<ApiResponse>
    {
        public int ClientId { get; set; }
        public int PageSize { get; set; }

        public int PageNo { get; set; }
        public LHSAPI.Common.Enums.Client.ClientFundingOrderBy OrderBy { get; set; }
        public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }

    }
}

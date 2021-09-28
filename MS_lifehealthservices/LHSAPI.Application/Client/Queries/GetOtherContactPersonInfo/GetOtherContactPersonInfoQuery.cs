
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetOtherContactPersonInfo
{
    public class GetOtherContactPersonInfoQuery : IRequest<ApiResponse>
    {
    public int ClientId { get; set; }
   
    public int PageSize{get;set;}

    public int PageNo{get;set;}
        public LHSAPI.Common.Enums.Client.ClientContactOrderBy OrderBy { get; set; }
        public LHSAPI.Common.Enums.SortOrder SortOrder { get; set; }

    }
}

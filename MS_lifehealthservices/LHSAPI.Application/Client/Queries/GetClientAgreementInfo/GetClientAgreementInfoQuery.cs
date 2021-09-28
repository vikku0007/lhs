
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetClientAgreementInfo
{
    public class GetClientAgreementInfoQuery : IRequest<ApiResponse>
    {
        public int ClientId { get; set; }
        public int PageSize { get; set; }

        public int PageNo { get; set; }
       

    }
}

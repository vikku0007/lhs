
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Administration.Queries.GetServiceRate
{
    public class GetServiceRateQuery : IRequest<ApiResponse>
    {
        public int PageSize { get; set; }

        public int PageNo { get; set; }

    }
}


using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetServiceRate
{
    public class GetServiceRateQuery : IRequest<ApiResponse>
    {
        public int Id { get; set; }

       
    }
}

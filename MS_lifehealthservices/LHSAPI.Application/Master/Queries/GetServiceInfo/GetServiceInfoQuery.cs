
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetServiceInfo
{
    public class GetServiceInfoQuery : IRequest<ApiResponse>
    {
        public int ItemNumber { get; set; }

       
    }
}

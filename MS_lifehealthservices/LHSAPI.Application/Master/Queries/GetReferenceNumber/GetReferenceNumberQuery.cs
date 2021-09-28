
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetReferenceNumber
{
    public class GetReferenceNumberQuery : IRequest<ApiResponse>
    {
        public int ClientId { get; set; }
    }
}

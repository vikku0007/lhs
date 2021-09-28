using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetClientPrimaryInfo
{
    public class GetClientPrimaryInfoQuery: IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}


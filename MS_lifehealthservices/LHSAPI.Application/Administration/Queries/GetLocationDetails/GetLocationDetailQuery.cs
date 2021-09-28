
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Administration.Queries.GetLocationDetails
{
    public class GetLocationDetailQuery : IRequest<ApiResponse>
    {


        public int LocationId { get; set; }

    }
}

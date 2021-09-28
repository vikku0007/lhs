
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetGlobalPayRate
{
    public class GetGlobalPayRate : IRequest<ApiResponse>
    {
        public int Level { get; set; }

       
    }
}


using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetSourceOfHire
{
    public class GetSourceOfHireQuery : IRequest<ApiResponse>
    {
        public string CodeData { get; set; }

        public string CodeDescription { get; set; }
    }
}


using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetPrimaryCategory
{
    public class GetPrimaryCategoryQuery : IRequest<ApiResponse>
    {
       
        public string CodeData { get; set; }

        public string CodeDescription { get; set; }
    }
}


using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetEmployeeCheckListdoc
{
    public class GetEmployeeCheckListdocQuery : IRequest<ApiResponse>
    {
       
        public int Id { get; set; }

        
    }
}

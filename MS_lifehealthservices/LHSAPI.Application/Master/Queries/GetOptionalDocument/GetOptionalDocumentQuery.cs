
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetOptionalDocument
{
    public class GetOptionalDocumentQuery : IRequest<ApiResponse>
    {
       
        public int Id { get; set; }

        
    }
}

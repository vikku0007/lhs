
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetClientDocuments
{
    public class GetClientDocumentsQuery : IRequest<ApiResponse>
    {
       
        public int DocumentType { get; set; }

    }
}

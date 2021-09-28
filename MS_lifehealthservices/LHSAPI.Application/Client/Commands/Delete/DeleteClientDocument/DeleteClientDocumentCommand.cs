using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Delete.DeleteClientDocument
{
    public class DeleteClientDocumentCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}

using LHSAPI.Common.ApiResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Update.UpdateIncidentDocument
{
    public class UpdateIncidentDocumentCommand : IRequest<ApiResponse>
    {
        public string Id { get; set; }
        public string ClientId { get; set; }

        public string DocumentName { get; set; }
 
        public string FileName { get; set; }
        public IFormFile files { get; set; }

    }
    
}

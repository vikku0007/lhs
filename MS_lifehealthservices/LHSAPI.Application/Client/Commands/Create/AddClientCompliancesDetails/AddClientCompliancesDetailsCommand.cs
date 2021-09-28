using LHSAPI.Common.ApiResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddClientCompliancesDetails
{
    public class AddClientCompliancesDetailsCommand : IRequest<ApiResponse>
    {
        public string ClientId { get; set; }

        public string DocumentName { get; set; }

        public string DocumentType { get; set; }

        public string IssueDate { get; set; }

        public string ExpiryDate { get; set; }

        public string Description { get; set; }

        public string HasExpiry { get; set; }

        public string Alert { get; set; }

        public string FileName { get; set; }
        public IFormFile files { get; set; }
        

    }
    
}

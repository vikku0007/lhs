using LHSAPI.Common.ApiResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddIncidentDocuments
{
    public class AddIncidentDocumentsCommand : IRequest<ApiResponse>
    {
        public string ClientId { get; set; }

        public string DocumentName { get; set; }
 
        public string FileName { get; set; }
        public IFormFile files { get; set; }
        public string ShiftId { get; set; }
        public string EmployeeId { get; set; }

    }
    
}

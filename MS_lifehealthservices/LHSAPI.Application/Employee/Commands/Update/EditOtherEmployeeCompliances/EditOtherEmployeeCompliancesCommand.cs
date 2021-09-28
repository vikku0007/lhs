using LHSAPI.Common.ApiResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Update.EditOtherEmployeeCompliances
{
  public class EditOtherEmployeeCompliancesCommand : IRequest<ApiResponse>
  {
        public string Id { get; set; }
        public string EmployeeId { get; set; }

        public string OtherDocumentName { get; set; }

        public string OtherDocumentType { get; set; }

        public string OtherIssueDate { get; set; }

        public string OtherExpiryDate { get; set; }

        public string OtherDescription { get; set; }

        public string OtherHasExpiry { get; set; }

        public string OtherAlert { get; set; }
        public string OtherFileName { get; set; }
        // public string FileName { get; set; }
        public IFormFile files { get; set; }

    }
}

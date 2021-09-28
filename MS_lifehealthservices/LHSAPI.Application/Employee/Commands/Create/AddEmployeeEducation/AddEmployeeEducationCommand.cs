using LHSAPI.Common.ApiResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeEducation
{
    public class AddEmployeeEducationCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }

        public string Institute { get; set; }

        public string Degree { get; set; }

        public string FieldStudy { get; set; }

        public string CompletionDate { get; set; }

        public string AdditionalNotes { get; set; }
        public string DocumentPath { get; set; }
        public IFormFile files { get; set; }
        public int? QualificationType { get; set; }

    }
}

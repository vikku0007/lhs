using LHSAPI.Common.ApiResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeWorkExp
{
    public class AddEmployeeWorkExpCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public string Company { get; set; }

        public string JobTitle { get; set; }

        public string JobDesc { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Duration { get; set; }
        public string DocumentPath { get; set; }
        public IFormFile files { get; set; }
    }
}

using LHSAPI.Common.ApiResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Update.UpdateEmployeeTraining
{
    public class UpdateEmployeeTrainingCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int MandatoryTraining { get; set; }

        public int TrainingType { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? CourseType { get; set; }

        public string Remarks { get; set; }
        public string IsAlert { get; set; }
        public string FileName { get; set; }
        public IFormFile files { get; set; }
        public string OtherTraining { get; set; }
    }
}

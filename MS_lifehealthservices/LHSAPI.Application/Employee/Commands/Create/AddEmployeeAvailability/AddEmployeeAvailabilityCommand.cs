using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeAvailability
{
  public class AddEmployeeAvailabilityCommand : IRequest<ApiResponse>
  {
        public int EmployeeId { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public bool? WorksMon { get; set; }
        public string AvailabilityDay { get; set; }
    }
}

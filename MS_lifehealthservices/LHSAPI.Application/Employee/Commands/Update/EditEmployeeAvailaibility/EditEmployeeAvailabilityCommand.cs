using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Update.EditEmployeeAvailability
{
  public class EditEmployeeAvailabilityCommand : IRequest<ApiResponse>
  {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public bool? WorksMon { get; set; }
        public string AvailabilityDay { get; set; }


    }
}

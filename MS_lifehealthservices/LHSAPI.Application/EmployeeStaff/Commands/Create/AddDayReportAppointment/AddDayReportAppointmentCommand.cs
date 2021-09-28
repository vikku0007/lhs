using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportAppointment
{
  public class AddDayReportAppointmentCommand : IRequest<ApiResponse>
  {
       
        public TimeSpan? Time { get; set; }

        public int ClientId { get; set; }

        public string Details { get; set; }

        public string GeneralInformation { get; set; }

        public string NightReport { get; set; }

        public int ShiftId { get; set; }

       


    }
}

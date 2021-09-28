using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportSupportWorker
{
  public class AddSupportWorkerCommand : IRequest<ApiResponse>
  {
       
        public int? StaffName { get; set; }

        public TimeSpan? TimeIn { get; set; }

        public TimeSpan? TimeOut { get; set; }

        public string Signature { get; set; }

        public int ShiftId { get; set; }


    }
}

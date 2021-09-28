using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateDayReportSupportWorker
{
  public class UpdateSupportWorkerCommand : IRequest<ApiResponse>
  {
       
        public int Id { get; set; }
        public int? StaffName { get; set; }

        public TimeSpan? TimeIn { get; set; }

        public TimeSpan? TimeOut { get; set; }

        public string Signature { get; set; }

        public int ShiftId { get; set; }


    }
}

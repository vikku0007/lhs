using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateDayReportTelephoneMsg
{
  public class UpdateTelephoneMsgCommand : IRequest<ApiResponse>
  {
        public int Id { get; set; }

        public TimeSpan? Time { get; set; }

        public string Caller { get; set; }

        public string MessageFor { get; set; }

        public string Message { get; set; }

        public string Signature { get; set; }

        public int ShiftId { get; set; }


    }
}

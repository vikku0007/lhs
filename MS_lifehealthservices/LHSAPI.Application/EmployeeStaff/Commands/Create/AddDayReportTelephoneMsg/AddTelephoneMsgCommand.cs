using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportTelephoneMsg
{
  public class AddTelephoneMsgCommand : IRequest<ApiResponse>
  {
       

        public TimeSpan? Time { get; set; }

        public string Caller { get; set; }

        public string MessageFor { get; set; }

        public string Message { get; set; }

        public string Signature { get; set; }

        public int ShiftId { get; set; }


    }
}

using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportVisitor
{
  public class AddDayReportVisitorCommand : IRequest<ApiResponse>
  {
       
        public TimeSpan? Time { get; set; }

        public string VisitorName { get; set; }

        public string VisitReason { get; set; }

        public int ShiftId { get; set; }


    }
}

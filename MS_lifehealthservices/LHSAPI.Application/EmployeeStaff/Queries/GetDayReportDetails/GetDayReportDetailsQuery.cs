
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.EmployeeStaff.Queries.GetDayReportDetails
{
    public class GetDayReportDetailsQuery : IRequest<ApiResponse>
    {
    
    public int ShiftId { get; set; }
  
  }
}

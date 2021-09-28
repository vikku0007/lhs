using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.TimeSheet.Queries.GetEmployeeHourReport
{
  public class GetEmployeeHourReport : IRequest<ApiResponse>
  {
    public int SearchByEmpId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
  }
}

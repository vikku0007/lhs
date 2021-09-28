using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Queries.GetEmployeeViewCalendar
{
  public class GetEmployeeViewCalendarQuery : IRequest<ApiResponse>
  {
    public int SearchByEmpName { get; set; }
    public int SearchByClientName { get; set; }

    public int SearchTextBylocation { get; set; }
    public int SearchTextByStatus { get; set; }
    public int SearchTextByShiftType { get; set; }
    public string SearchTextByManualAddress { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }


  }
}

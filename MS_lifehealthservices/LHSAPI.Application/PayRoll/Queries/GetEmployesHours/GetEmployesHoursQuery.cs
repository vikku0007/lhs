using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.PayRoll.Queries.GetEmployesHours
{
  public class GetEmployesHoursQuery : IRequest<ApiResponse>
  {
    public int SearchByEmpName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsOnTime { get; set; }

  }
}

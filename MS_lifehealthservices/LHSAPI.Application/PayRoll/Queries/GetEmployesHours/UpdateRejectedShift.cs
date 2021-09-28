using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.PayRoll.Queries.GetEmployesHours
{
  public class UpdateRejectedShift : IRequest<ApiResponse>
  {
    public int EmployeeTrackerId { get; set; }
  
   
  }
}

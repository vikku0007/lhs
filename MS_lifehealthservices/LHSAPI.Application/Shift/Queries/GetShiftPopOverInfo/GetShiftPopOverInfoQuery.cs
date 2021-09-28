using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Queries.GetShiftPopOverInfo
{
  public class GetShiftPopOverInfoQuery : IRequest<ApiResponse>
  { 
    public int ShiftId { get; set; }

  }
}

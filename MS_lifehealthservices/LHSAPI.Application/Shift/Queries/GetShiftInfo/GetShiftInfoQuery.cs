using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Queries.GetShiftInfo
{
  public class GetShiftInfoQuery : IRequest<ApiResponse>
  { 
    public int Id { get; set; }

  }
}

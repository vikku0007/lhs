using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Queries.GetShifToDoList
{
  public class GetShifToDoListQuery : IRequest<ApiResponse>
  { 
    public int ShiftId { get; set; }

  }
}

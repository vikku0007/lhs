using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Commands.Update.UpdateShiftStatus
{
  public class UpdateShiftStatusCommand : IRequest<ApiResponse>
  {
    public int Id { get; set; }

    public int StatusId { get; set; }

   
  }
}

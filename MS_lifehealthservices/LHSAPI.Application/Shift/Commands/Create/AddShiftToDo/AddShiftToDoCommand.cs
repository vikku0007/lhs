using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Commands.Create.AddShiftToDo
{
  public class AddShiftToDoCommand : IRequest<ApiResponse>
  {

    public int ShiftId { get; set; }

    public int EmployeeId { get; set; }

    public string Description { get; set; }
  }
}

using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Commands.Create.AddShiftTemplate
{
  public class AddShiftTemplateCommand : IRequest<ApiResponse>
  {

    public List<int> ShiftId { get; set; }

    public string Name { get; set; }
  }
}

using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Shift.Queries.GetShiftTemplate
{
  public class GetShiftTemplateQuery : IRequest<ApiResponse>
  {
  
    public int ShiftTemplateId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

  }
}

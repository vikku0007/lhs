
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetIncidentDetails
{
    public class GetIncidentDetailsQuery : IRequest<ApiResponse>
    {
    public int Id { get; set; }
    public int ShiftId { get; set; }
  
  }
}

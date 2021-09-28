
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetIncidentPrimaryContact
{
    public class GetIncidentPrimaryContactQuery : IRequest<ApiResponse>
    {
    public int Id { get; set; }
    public int ShiftId { get; set; }
    

  }
}

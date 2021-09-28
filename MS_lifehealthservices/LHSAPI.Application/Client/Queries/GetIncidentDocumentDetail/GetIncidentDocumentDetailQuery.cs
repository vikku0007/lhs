
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetIncidentDocumentDetail
{
    public class GetIncidentDocumentDetailQuery : IRequest<ApiResponse>
    {
    public int Id { get; set; }
    public int ShiftId { get; set; }
    

  }
}

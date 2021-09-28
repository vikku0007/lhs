
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetClientMedicalInfo
{
    public class GetClientMedicalInfoQuery : IRequest<ApiResponse>
    {
    public int Id { get; set; }
    
    

  }
}


using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetClientAccidentDetails
{
    public class GetClientAccidentDetailsQuery : IRequest<ApiResponse>
    {
    public int Id { get; set; }
    
   

  }
}

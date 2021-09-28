
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetClientProfileDetails
{
    public class GetClientProfileDetailsQuery : IRequest<ApiResponse>
    {
    public int Id { get; set; }
    

  }
}

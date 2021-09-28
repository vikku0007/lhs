
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetClientDetailsById
{
    public class GetClientDetailQuery : IRequest<ApiResponse>
    {
    public int Id { get; set; }
    

  }
}

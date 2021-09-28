
using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Queries.GetImageDetail
{
    public class GetImageDetailQuery : IRequest<ApiResponse>
    {
    public int UserRole { get; set; }
    public int Id { get; set; }
    

  }
}

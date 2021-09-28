using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace LHSAPI.Application.Administration.Commands.Create.UploadServicePrice
{
  public class UploadServicePriceCommand : IRequest<ApiResponse>
  {
        public IFormFile files { get; set; }

    }
}

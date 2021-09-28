using LHSAPI.Common.ApiResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddClientPicInfo
{
  public class AddClientPicInfoCommand : IRequest<ApiResponse>
  {
    public string ClientId { get; set; }

   public IFormFile files { get; set; }


    }
}

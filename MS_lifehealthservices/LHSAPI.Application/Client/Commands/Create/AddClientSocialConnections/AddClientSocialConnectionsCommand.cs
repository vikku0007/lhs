using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddClientSocialConnections
{
  public class AddClientSocialConnectionsCommand : IRequest<ApiResponse>
  {
        public int Id { get; set; }
        public int ClientId { get; set; }

        public string SocialConnection { get; set; }


    }
}

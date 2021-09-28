using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Update.UpdateClientActiveStatus
{
  public class UpdateClientActiveStatusCommand : IRequest<ApiResponse>
  {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public bool Status { get; set; }
        


    }
}

using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddClientAdditionalNotes
{
  public class AddClientAdditionalNotesCommand : IRequest<ApiResponse>
  {
        public int ClientId { get; set; }

        public string PrivateNote { get; set; }

        public string PublicNote { get; set; }

    }
}

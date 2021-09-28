using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddClientBoardingNotes
{
  public class AddClientBoardingNotesCommand : IRequest<ApiResponse>
  {
       
        public int ClientId { get; set; }

        public string CareNote { get; set; }

        public string CareNoteByClient { get; set; }
    }
}

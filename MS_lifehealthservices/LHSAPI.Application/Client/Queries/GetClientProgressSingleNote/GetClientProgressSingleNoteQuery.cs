using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetClientProgressSingleNote
{
    public class GetClientProgressSingleNoteQuery : IRequest<ApiResponse>
    {
        public int ClientId { get; set; }
    }
}


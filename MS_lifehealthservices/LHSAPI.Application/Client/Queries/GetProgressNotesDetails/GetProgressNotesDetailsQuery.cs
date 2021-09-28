using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Queries.GetProgressNotesDetails
{
    public class GetProgressNotesDetailsQuery : IRequest<ApiResponse>
    {
        public int? Id { get; set; }
        public int? ClientId { get; set; }
    }
}

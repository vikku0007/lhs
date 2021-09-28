using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Administration.Commands.Delete.DeleteLocation
{
    public class DeleteLocationCommand : IRequest<ApiResponse>
    {
        public int LocationId { get; set; }
    }
}

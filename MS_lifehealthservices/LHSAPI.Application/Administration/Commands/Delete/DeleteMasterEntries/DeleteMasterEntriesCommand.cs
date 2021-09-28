using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Administration.Commands.Delete.DeleteMasterEntries
{
    public class DeleteMasterEntriesCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}

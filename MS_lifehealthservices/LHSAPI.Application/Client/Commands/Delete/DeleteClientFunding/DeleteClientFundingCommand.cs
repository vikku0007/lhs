using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Delete.DeleteClientFunding
{
    public class DeleteClientFundingCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}

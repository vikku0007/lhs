using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Delete.DeleteClientAgreement
{
    public class DeleteClientAgreementCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}

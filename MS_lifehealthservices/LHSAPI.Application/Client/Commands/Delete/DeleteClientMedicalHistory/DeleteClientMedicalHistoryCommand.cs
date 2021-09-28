using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Delete.DeleteClientMedicalHistory
{
    public class DeleteClientMedicalHistoryCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}

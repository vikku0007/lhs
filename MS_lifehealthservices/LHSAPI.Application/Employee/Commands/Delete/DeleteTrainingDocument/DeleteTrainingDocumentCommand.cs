using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteTrainingDocument
{
    public class DeleteTrainingDocumentCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}

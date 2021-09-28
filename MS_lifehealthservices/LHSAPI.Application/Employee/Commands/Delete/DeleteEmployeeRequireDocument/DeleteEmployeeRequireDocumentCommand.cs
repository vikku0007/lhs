using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeRequireDocument
{
    public class DeleteEmployeeRequireDocumentCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}

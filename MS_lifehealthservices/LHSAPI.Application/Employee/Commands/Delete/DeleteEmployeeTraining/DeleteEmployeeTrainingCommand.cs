using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeTraining
{
    public class DeleteEmployeeTrainingCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
    }
}

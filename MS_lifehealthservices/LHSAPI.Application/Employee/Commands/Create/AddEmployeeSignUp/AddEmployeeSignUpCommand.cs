using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeSignUp
{
    public class AddEmployeeSignUpCommand : IRequest<ApiResponse>
    {
        public int EmployeeId { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }

    }
}

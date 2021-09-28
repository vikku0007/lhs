using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Employee.Commands.Create.AddClientSignUp
{
    public class ClientResetPasswordCommand : IRequest<ApiResponse>
    {
        public int ClientId { get; set; }
        public string Password { get; set; }

    }
}

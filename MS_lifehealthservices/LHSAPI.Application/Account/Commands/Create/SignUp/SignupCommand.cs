
using MediatR;
using LHSAPI.Common.ApiResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Account.Commands.Create.SignUp
{
    public class SignupCommand : IRequest<ApiResponse>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        //public string SSN { get; set; }
    }
}

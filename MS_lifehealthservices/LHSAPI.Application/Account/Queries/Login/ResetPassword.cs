using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace LHSAPI.Application.Account.Queries.Login
{
    public class ResetPassword :  IRequest<object>
    {
    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
  }
}

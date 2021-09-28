using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace LHSAPI.Application.Account.Queries.Login
{
    public class ForgotPassword :  IRequest<object>
    {
    public string ForgetPasswordEmailId { get; set; }
  }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.ValidateToken
{
    public class ValidateTokenQuery : IRequest<object>
    {
        public string UserId { get; set; }
    }
}

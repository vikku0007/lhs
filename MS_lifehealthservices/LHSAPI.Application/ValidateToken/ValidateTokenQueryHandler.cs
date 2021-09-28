
using MediatR;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LHSAPI.Application.ValidateToken
{
    public class ValidateTokenQueryHandler : IRequestHandler<ValidateTokenQuery, object>
    {
        private readonly LHSDbContext _context;

        public ValidateTokenQueryHandler(LHSDbContext context)
        {
            _context = context;

        }

        public async Task<object> Handle(ValidateTokenQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var userDetails =  _context.User.FirstOrDefault(x => x.UserById == request.UserId);
                if (userDetails != null)
                {
                    if (userDetails.Token != null && userDetails.Token != "")
                    {
                        response.StatusCode = ResponseCode.Ok;
                    }
                    else
                    {
                        response.StatusCode = ResponseCode.BadRequest;
                        response.Message = ResponseMessage.Expire;

                    }
                }
            }
            catch (Exception)
            {
                response.StatusCode = ResponseCode.BadRequest;
                response.Message = ResponseMessage.Error;
            }
            return response;
        }


    }
}

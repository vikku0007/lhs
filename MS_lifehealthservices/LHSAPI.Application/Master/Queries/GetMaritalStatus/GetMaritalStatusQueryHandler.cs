
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using static LHSAPI.Common.Enums.ResponseEnums;
using LHSAPI.Domain.Entities;
using LHSAPI.Common.Enums;

namespace LHSAPI.Application.Master.Queries.GetMaritalStatus
{
    public class GetMaritalStatusQueryHandler : IRequestHandler<GetMaritalStatusQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetMaritalStatusQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
        #region My Leagues
        /// <summary>
        /// Get List Of All Leagues Of Particular User
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Handle(GetMaritalStatusQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var statuslist = (from status in _dbContext.StandardCode
                                  where status.CodeData == Common.Enums.ResponseEnums.StandardCode.MaritalStatus.ToString() &&  status.IsActive == true
                                  select new
                                  {
                                     status.ID,
                                     status.CodeDescription

                                  }).ToList();
                if (statuslist != null && statuslist.Any())
                {

                    response.SuccessWithOutMessage(statuslist.ToList());

                }
                else
                {
                    response.NotFound();
                }
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }
        #endregion
    }
}











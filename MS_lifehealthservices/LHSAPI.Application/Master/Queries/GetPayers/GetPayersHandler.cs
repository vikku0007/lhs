using LHSAPI.Application.Master.Queries.GetEventType;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace LHSAPI.Application.Master.Queries.GetPayers
{
    public class GetPayersHandler : IRequestHandler<GetPayersQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetPayersHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;            
        }

        #region My Leagues
        /// <summary>
        /// Get List Of All Leagues Of Particular User
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Handle(GetPayersQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var fundList = (from fundtype in _dbContext.StandardCode
                                 where fundtype.CodeData == Common.Enums.ResponseEnums.StandardCode.Payers.ToString() && fundtype.IsActive == true
                                 select new
                                 {
                                     fundtype.ID,
                                     fundtype.CodeDescription

                                 }).ToList();
                if (fundList != null && fundList.Any())
                {

                    response.SuccessWithOutMessage(fundList.ToList());

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

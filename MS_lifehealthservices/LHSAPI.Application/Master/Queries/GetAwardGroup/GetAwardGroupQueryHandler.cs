
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

namespace LHSAPI.Application.Master.Queries.GetAwardGroup
{
    public class GetGlobalPayRateQueryHandler : IRequestHandler<GetAwardGroupQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetGlobalPayRateQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetAwardGroupQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var Genderlist = (from gender in _dbContext.StandardCode
                                  where gender.CodeData == Common.Enums.ResponseEnums.StandardCode.AwardGroup.ToString() &&
                                     gender.IsActive == true
                                  select new
                                  {
                                     gender.ID,
                                     gender.CodeDescription

                                  }).ToList();
                if (Genderlist != null && Genderlist.Any())
                {

                    response.SuccessWithOutMessage(Genderlist.ToList());

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











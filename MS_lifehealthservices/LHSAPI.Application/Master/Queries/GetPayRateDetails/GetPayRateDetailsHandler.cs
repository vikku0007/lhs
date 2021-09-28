
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

namespace LHSAPI.Application.Master.Queries.GetPayRateDetails
{
    public class GetPayRateDetailsHandler : IRequestHandler<GetPayRateDetails, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetPayRateDetailsHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetPayRateDetails request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var Genderlist = (from gender in _dbContext.GlobalPayRate
                                  where gender.IsActive == true 
                                  select new
                                  {
                                    gender.Id,
                                    gender.Level,
                                    gender.MonToFri12To6AM,
                                    gender.Sat12To6AM,
                                    gender.Sun12To6AM,
                                    gender.Holiday12To6AM,
                                    gender.MonToFri6To12AM,
                                    gender.Sat6To12AM,
                                    gender.Sun6To12AM,
                                    gender.Holiday6To12AM,
                                    gender.ActiveNightsAndSleep,
                                    gender.HouseCleaning,
                                    gender.TransportPetrol,
                                    LevelName= _dbContext.StandardCode.Where(x => x.Value == gender.Level).Select(x => x.CodeDescription).FirstOrDefault()

                                  }).ToList();
                if (Genderlist != null)
                {
                    var totalCount = Genderlist.Count();
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(Genderlist);

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











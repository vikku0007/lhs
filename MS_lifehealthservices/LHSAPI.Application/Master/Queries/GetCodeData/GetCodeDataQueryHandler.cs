
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

namespace LHSAPI.Application.Master.Queries.GetCodeData
{
    public class GetCodeDataQueryHandler : IRequestHandler<GetCodeDataQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetCodeDataQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetCodeDataQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var eventlist = (from eventtype in _dbContext.StandardCode
                                  where  eventtype.IsActive == true && eventtype.IsDeleted ==false
                                  select new
                                  {
                                      eventtype.ID,
                                      eventtype.CodeData

                                  }).OrderByDescending(x=>x.ID).ToList();
                var uniqlist = eventlist.GroupBy(x => x.CodeData).Select(y => y.First()).Distinct().ToList();

                if (uniqlist != null && uniqlist.Any())
                {

                    response.SuccessWithOutMessage(uniqlist.ToList());

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











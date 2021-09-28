
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

namespace LHSAPI.Application.Master.Queries.GetCommunicationType
{
    public class GetCommunicationTypeHandler : IRequestHandler<GetCommunicationTypeQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetCommunicationTypeHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetCommunicationTypeQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var appraisallist = (from appraisaltype in _dbContext.StandardCode
                                  where appraisaltype.CodeData == Common.Enums.ResponseEnums.StandardCode.CommunicationType.ToString() && appraisaltype.IsActive == true
                                  select new
                                  {
                                      appraisaltype.ID,
                                      appraisaltype.CodeDescription

                                  }).ToList();
                if (appraisallist != null && appraisallist.Any())
                {

                    response.SuccessWithOutMessage(appraisallist.ToList());

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











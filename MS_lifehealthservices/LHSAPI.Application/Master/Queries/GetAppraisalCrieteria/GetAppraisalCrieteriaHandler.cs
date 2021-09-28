
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

namespace LHSAPI.Application.Master.Queries.GetAppraisalCrieteria
{
    public class GetAppraisalCrieteriaHandler : IRequestHandler<GetAppraisalCrieteriaQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetAppraisalCrieteriaHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetAppraisalCrieteriaQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();

            try
            {
                var Emplist = (from Emptype in _dbContext.StandardCode where Emptype.CodeData == Common.Enums.ResponseEnums.StandardCode.AppraisalCrieteria.ToString() && Emptype.IsActive == true
                              select new
                            {
                               Emptype.ID,
                               Description= Emptype.CodeDescription

                           }).ToList();

                if (Emplist != null && Emplist.Any())
                {

                    response.SuccessWithOutMessage(Emplist);

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











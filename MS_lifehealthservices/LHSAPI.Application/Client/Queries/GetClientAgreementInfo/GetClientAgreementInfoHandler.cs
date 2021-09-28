
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

using System.Text.Json;
using LHSAPI.Application.Client.Models;

namespace LHSAPI.Application.Client.Queries.GetClientAgreementInfo
{
    public class GetClientAgreementInfoHandler : IRequestHandler<GetClientAgreementInfoQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetClientAgreementInfoHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetClientAgreementInfoQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientDetails _clientDetails = new ClientDetails();
                var clientFundinglist = (from item in _dbContext.ClientFundingInfo
                                         where item.IsActive == true && item.IsDeleted == false && item.ClientId == request.ClientId
                                         select new LHSAPI.Application.Client.Models.ClientFundingInfo
                                         {

                                             Id = item.Id,
                                             ClientId=item.ClientId,
                                             FundType = item.FundType,
                                             RefNumber = item.RefNumber,
                                             Other = item.Other,
                                             StartDate = item.StartDate,
                                             EndDate = item.EndDate,
                                             FundTypeName = _dbContext.StandardCode.Where(x => x.ID == (item.FundType)).Select(x => x.CodeDescription).FirstOrDefault(),
                                             Amount = item.Amount,
                                             ClaimNumber = item.ClaimNumber

                                         }).ToList();

                if (clientFundinglist != null && clientFundinglist.Any())
                {
                    var totalCount = clientFundinglist.Count();
                    var clientlist = clientFundinglist.OrderByDescending(x => x.Id).ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(clientlist);



                }
                else
                {
                    response = response.NotFound();
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

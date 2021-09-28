
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

namespace LHSAPI.Application.Client.Queries.GetClientFundingTypeList
{
    public class GetClientFundingTypeListQueryHandler : IRequestHandler<GetClientFundingTypeListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetClientFundingTypeListQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetClientFundingTypeListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientDetails _clientDetails = new ClientDetails();
                List<ClientFunding> clientFundinglist = new List<ClientFunding>();
                var commList = _dbContext.ClientFunding.Where(x => x.ClientId == request.ClientId && x.IsActive == true
                  && x.IsDeleted == false).AsQueryable().OrderByDescending(x => x.Id).ToList();
                if (commList != null && commList.Count > 0)
                {

                    foreach (var item in commList)
                    {
                        ClientFunding clientFunding = new ClientFunding
                        {
                            Id = item.Id,
                            Ammount = (item.Ammount),
                            TotalAmount = (item.TotalAmount),
                            NoDays = item.NoDays,
                            ServiceType = Convert.ToInt32(item.ServiceType),
                            ServiceTypeName = _dbContext.ServiceDetails.Where(x => x.Id == Convert.ToInt32(item.ServiceType)).Select(x => x.SupportItemName).FirstOrDefault(),
                            ClaimNumber =item.ClaimNumber,
                            CreatedDate=item.CreatedDate,
                            PaymentTerm=item.PaymentTerm,
                            Payer=item.Payer,
                            ReferenceNumber=item.ReferenceNumber,
                            PaymentTermName = _dbContext.StandardCode.Where(x => x.ID == item.PaymentTerm).Select(x => x.CodeDescription).FirstOrDefault(),
                            PayerName = _dbContext.StandardCode.Where(x => x.ID == item.Payer).Select(x => x.CodeDescription).FirstOrDefault(),
                        };
                        clientFundinglist.Add(clientFunding);

                    }

                    if (clientFundinglist != null && clientFundinglist.Any())
                    {
                        var totalCount = clientFundinglist.Count();

                        switch (request.OrderBy)
                        {
                            case Common.Enums.Client.ClientFundingOrderBy.ServiceType:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.ServiceType).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.ServiceType).ToList();
                                }
                                break;
                            case Common.Enums.Client.ClientFundingOrderBy.NoDays:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.NoDays).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.NoDays).ToList();
                                }
                                break;
                            case Common.Enums.Client.ClientFundingOrderBy.Ammount:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.Ammount).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.Ammount).ToList();
                                }
                                break;

                            case Common.Enums.Client.ClientFundingOrderBy.TotalAmount:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.TotalAmount).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.TotalAmount).ToList();
                                }
                                break;
                            
                            default:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.CreatedDate).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.CreatedDate).ToList();
                                }

                                break;
                        }


                        //empList = empList.Skip<EmployeePrimaryInfo>((request.PageNo > 0 ? (request.PageNo - 1) : request.PageNo) * request.PageSize).Take<EmployeePrimaryInfo>(request.PageSize).ToList();
                        var clientlist = clientFundinglist.ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                        response.Total = totalCount;
                        response.SuccessWithOutMessage(clientlist);



                    }
                    else
                    {
                        response = response.NotFound();
                    }

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

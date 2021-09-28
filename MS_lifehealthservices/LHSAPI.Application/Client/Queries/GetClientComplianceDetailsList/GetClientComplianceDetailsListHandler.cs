
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

namespace LHSAPI.Application.Client.Queries.GetAllComplianceDetailsList
{
    public class GetClientComplianceDetailsListHandler : IRequestHandler<GetClientComplianceDetailsListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetClientComplianceDetailsListHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetClientComplianceDetailsListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                var AvbempList = (from comp in _dbContext.ClientCompliancesDetails
                                  where comp.IsActive == true && comp.IsDeleted == false && comp.ClientId == request.ClientId 
                                  select new LHSAPI.Application.Client.Models.ClientComplianceModel
                                  {
                                      Id = comp.Id,
                                      DocumentName = comp.DocumentName,
                                      DocumentType = comp.DocumentType,
                                      ExpiryDate = comp.ExpiryDate,
                                      IssueDate = comp.IssueDate,
                                      Alert = comp.Alert,
                                      HasExpiry = comp.HasExpiry,
                                      Description = comp.Description,
                                      DocumentTypeName = _dbContext.StandardCode.Where(x => x.ID == comp.DocumentType).Select(x => x.CodeDescription).FirstOrDefault(),
                                      Document = _dbContext.StandardCode.Where(x => x.ID == comp.DocumentName).Select(x => x.CodeDescription).FirstOrDefault(),
                                      FileName = comp.FileName,
                                      CreatedDate = comp.CreatedDate
                                  });

                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Client.ClientComplianceOrderBy.documentType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.DocumentType);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.DocumentType);
                            }
                            break;
                        case Common.Enums.Client.ClientComplianceOrderBy.documentName:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.DocumentName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.DocumentName);
                            }
                            break;
                        case Common.Enums.Client.ClientComplianceOrderBy.description:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Description);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Description);
                            }
                            break;


                        case Common.Enums.Client.ClientComplianceOrderBy.hasExpiry:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.HasExpiry);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Alert);
                            }
                            break;
                        case Common.Enums.Client.ClientComplianceOrderBy.dateOfExpiry:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.ExpiryDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.ExpiryDate);
                            }
                            break;
                        case Common.Enums.Client.ClientComplianceOrderBy.alert:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Alert);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Alert);
                            }
                            break;
                        default:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.CreatedDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.CreatedDate);
                            }

                            break;
                    }

                    var clientlist = AvbempList.ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
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

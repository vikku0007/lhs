
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

namespace LHSAPI.Application.Client.Queries.GetAllClientDocumentsList
{
    public class GetAllClientDocumentsListHandler : IRequestHandler<GetAllClientDocumentsListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetAllClientDocumentsListHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetAllClientDocumentsListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var AvbempList = (from Clientdata in _dbContext.ClientPrimaryInfo
                                  join RequireComp in _dbContext.ClientCompliancesDetails on Clientdata.Id equals RequireComp.ClientId
                                  where RequireComp.IsDeleted == false && RequireComp.IsActive == true && Clientdata.IsDeleted == false && Clientdata.IsActive == true && (string.IsNullOrEmpty(request.SearchTextByName) || Clientdata.FirstName.Contains(request.SearchTextByName) || Clientdata.LastName.Contains(request.SearchTextByName))
                                  select new
                                  {
                                      Id = RequireComp.Id,
                                      Alert = RequireComp.Alert,
                                      DocumentName = RequireComp.DocumentName,
                                      DocumentType = RequireComp.DocumentType,
                                      DocumentTypeName = _dbContext.StandardCode.Where(x => x.ID == RequireComp.DocumentType).Select(x => x.CodeDescription).FirstOrDefault(),
                                      Description = RequireComp.Description,
                                      IssueDate = RequireComp.IssueDate,
                                      HasExpiry = RequireComp.HasExpiry,
                                      ExpiryDate = RequireComp.ExpiryDate,
                                      FirstName = Clientdata.FirstName,
                                      MiddleName = Clientdata.MiddleName,
                                      LastName = Clientdata.LastName,
                                      FullName = Clientdata.FirstName + " " + ((Clientdata.MiddleName == null) ? "" : " " + Clientdata.MiddleName) + " " + ((Clientdata.LastName == null) ? "" : " " + Clientdata.LastName),
                                      Document = _dbContext.ClientDocumentName.Where(x => x.Id == RequireComp.DocumentName).Select(x => x.DocumentName).FirstOrDefault(),
                                      ClientId = RequireComp.ClientId,
                                      FileName=RequireComp.FileName,
                                      CreatedDate = RequireComp.CreatedDate
                                  });
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Client.ClientDocumentListOrderBy.clientName:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.FullName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.FullName);
                            }
                            break;
                        case Common.Enums.Client.ClientDocumentListOrderBy.documentType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.DocumentType);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.DocumentType);
                            }
                            break;
                        case Common.Enums.Client.ClientDocumentListOrderBy.documentName:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.DocumentName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.DocumentName);
                            }
                            break;
                        case Common.Enums.Client.ClientDocumentListOrderBy.description:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Description);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Description);
                            }
                            break;
                        case Common.Enums.Client.ClientDocumentListOrderBy.dateofissue:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.IssueDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.IssueDate);
                            }
                            break;

                        case Common.Enums.Client.ClientDocumentListOrderBy.hasExpiry:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.HasExpiry);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.HasExpiry);
                            }
                            break;
                        case Common.Enums.Client.ClientDocumentListOrderBy.dateOfExpiry:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.ExpiryDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.ExpiryDate);
                            }
                            break;
                        case Common.Enums.Client.ClientDocumentListOrderBy.alert:
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

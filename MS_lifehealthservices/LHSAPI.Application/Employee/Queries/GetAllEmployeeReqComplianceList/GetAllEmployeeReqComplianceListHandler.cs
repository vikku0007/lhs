
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

namespace LHSAPI.Application.Employee.Queries.GetAllEmployeeReqComplianceList
{
    public class GetAllEmployeeReqComplianceListHandler : IRequestHandler<GetAllEmployeeReqComplianceListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetAllEmployeeReqComplianceListHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetAllEmployeeReqComplianceListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var AvbempList = (from Employeedata in _dbContext.EmployeePrimaryInfo
                                  join RequireComp in _dbContext.EmployeeCompliancesDetails on Employeedata.Id equals RequireComp.EmployeeId
                                  where RequireComp.IsDeleted == false && RequireComp.IsActive == true && Employeedata.IsDeleted == false && Employeedata.IsActive == true && (string.IsNullOrEmpty(request.SearchTextByName) || Employeedata.FirstName.Contains(request.SearchTextByName) || Employeedata.LastName.Contains(request.SearchTextByName))
                                  select new
                                  {
                                      Id = RequireComp.Id,
                                      Alert = RequireComp.Alert,
                                      DocumentName = RequireComp.DocumentName,
                                      //  DocumentType = RequireComp.DocumentType,
                                      //  DocumentTypeName = _dbContext.StandardCode.Where(x => x.ID == RequireComp.DocumentType).Select(x => x.CodeDescription).FirstOrDefault(),
                                      Description = RequireComp.Description,
                                      IssueDate = RequireComp.IssueDate,
                                      HasExpiry = RequireComp.HasExpiry,
                                      ExpiryDate = RequireComp.ExpiryDate,
                                      FirstName = Employeedata.FirstName,
                                      MiddleName = Employeedata.MiddleName,
                                      LastName = Employeedata.LastName,
                                      FullName = Employeedata.FirstName + " " + ((Employeedata.MiddleName == null) ? "" : " " + Employeedata.MiddleName) + " " + ((Employeedata.LastName == null) ? "" : " " + Employeedata.LastName),
                                      EmployeeId = RequireComp.EmployeeId,
                                      Document = _dbContext.StandardCode.Where(x => x.ID == RequireComp.DocumentName).Select(x => x.CodeDescription).FirstOrDefault(),
                                      FileName = RequireComp.FileName,
                                      CreatedDate = RequireComp.CreatedDate
                                  });
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Employee.EmployeeComplianceOrderBy.Name:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.FullName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.FullName);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeComplianceOrderBy.document:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.DocumentName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.DocumentName);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeComplianceOrderBy.description:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Description);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Description);
                            }
                            break;
                        

                        case Common.Enums.Employee.EmployeeComplianceOrderBy.hasExpiry:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.HasExpiry);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Alert);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeComplianceOrderBy.dateOfExpiry:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.ExpiryDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.ExpiryDate);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeComplianceOrderBy.alert:
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

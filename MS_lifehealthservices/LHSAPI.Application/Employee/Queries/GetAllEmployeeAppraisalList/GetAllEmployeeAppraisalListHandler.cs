
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

namespace LHSAPI.Application.Employee.Queries.GetAllEmployeeAppraisalList
{
    public class GetAllEmployeeAppraisalListQueryHandler : IRequestHandler<GetAllEmployeeAppraisalListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetAllEmployeeAppraisalListQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetAllEmployeeAppraisalListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var AvbempList = (from Employeedata in _dbContext.EmployeePrimaryInfo
                                  join RequireComp in _dbContext.EmployeeAppraisalDetails on Employeedata.Id equals RequireComp.EmployeeId
                                 // join job in _dbContext.EmployeeJobProfile on RequireComp.Id equals job.EmployeeId
                                  where RequireComp.IsDeleted == false && RequireComp.IsActive == true && Employeedata.IsDeleted == false && Employeedata.IsActive == true && (string.IsNullOrEmpty(request.SearchTextByName) || Employeedata.FirstName.Contains(request.SearchTextByName) || Employeedata.LastName.Contains(request.SearchTextByName)) && (string.IsNullOrEmpty(request.SearchTextByEmpId))
                                  select new
                                  {
                                      RequireComp,
                                      Id=RequireComp.Id,
                                      Employeedata.FirstName,
                                      Employeedata.MiddleName,
                                      Employeedata.LastName,
                                      Employeedata.EmployeeId,
                                      //job.DepartmentId,
                                      FullName = Employeedata.FirstName + " " + ((Employeedata.MiddleName == null) ? "" : " " + Employeedata.MiddleName) + " " + ((Employeedata.LastName == null) ? "" : " " + Employeedata.LastName),
                                      AppraisalTypeName = _dbContext.StandardCode.Where(x => x.ID == RequireComp.AppraisalType).Select(x => x.CodeDescription).FirstOrDefault(),
                                      CreatedDate=RequireComp.CreatedDate
                                  });
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Employee.EmployeeAppraisalOrderBy.Name:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.FullName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.FullName);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAppraisalOrderBy.EmployeedetailId:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.RequireComp.EmployeeId);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.RequireComp.EmployeeId);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAppraisalOrderBy.AppraisalType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.RequireComp.AppraisalType);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.RequireComp.AppraisalType);
                            }
                            break;

                        case Common.Enums.Employee.EmployeeAppraisalOrderBy.AppraisalDateFrom:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.RequireComp.AppraisalDateFrom);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.RequireComp.AppraisalDateFrom);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAppraisalOrderBy.AppraisalDateTo:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.RequireComp.AppraisalDateTo);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.RequireComp.AppraisalDateTo
                                );
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

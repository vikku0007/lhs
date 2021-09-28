
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

namespace LHSAPI.Application.Employee.Queries.GetAllEmployeeStaffwarningList
{
    public class GetAllEmployeeStaffwarningListQueryHandler : IRequestHandler<GetAllEmployeeStaffwarningListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetAllEmployeeStaffwarningListQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetAllEmployeeStaffwarningListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                List<EmployeeStaffWarning> liststff = new List<EmployeeStaffWarning>();
                var AvbempList = (from Employeedata in _dbContext.EmployeePrimaryInfo
                                  join RequireComp in _dbContext.EmployeeStaffWarning on Employeedata.Id equals RequireComp.EmployeeId
                                  where RequireComp.IsDeleted == false && RequireComp.IsActive == true && (string.IsNullOrEmpty(request.SearchTextByName) || Employeedata.FirstName.Contains(request.SearchTextByName) || Employeedata.LastName.Contains(request.SearchTextByName))
                                  select new
                                  {
                                      RequireComp,
                                      Id=RequireComp.Id,
                                      Employeedata.FirstName,
                                      Employeedata.MiddleName,
                                      Employeedata.LastName,
                                      FullName = Employeedata.FirstName + " " + ((Employeedata.MiddleName == null) ? "" : " " + Employeedata.MiddleName) + " " + ((Employeedata.LastName == null) ? "" : " " + Employeedata.LastName),
                                      Warning = _dbContext.StandardCode.Where(x => x.ID == RequireComp.WarningType).Select(x => x.CodeDescription).FirstOrDefault(),
                                      OffenseTypeName = _dbContext.StandardCode.Where(x => x.ID == RequireComp.OffensesType).Select(x => x.CodeDescription).FirstOrDefault(),
                                     CreatedDate= RequireComp.CreatedDate
                                  });
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Employee.EmployeeStaffOrderBy.Name:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.FullName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.FullName);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeStaffOrderBy.warningType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.RequireComp.WarningType);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.RequireComp.WarningType);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeStaffOrderBy.description:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.RequireComp.Description);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.RequireComp.Description);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeStaffOrderBy.improvementPlan:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.RequireComp.ImprovementPlan);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.RequireComp.ImprovementPlan);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeStaffOrderBy.offensestype:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.OffenseTypeName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.OffenseTypeName);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeStaffOrderBy.otheroffenses:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.RequireComp.OtherOffenses);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.RequireComp.OtherOffenses);
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

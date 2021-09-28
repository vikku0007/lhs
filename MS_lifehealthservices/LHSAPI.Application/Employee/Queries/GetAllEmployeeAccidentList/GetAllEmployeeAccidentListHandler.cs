
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

namespace LHSAPI.Application.Employee.Queries.GetAllEmployeeAccidentList
{
    public class GetAllEmployeeCommunicationListHandler : IRequestHandler<GetAllEmployeeAccidentListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetAllEmployeeCommunicationListHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetAllEmployeeAccidentListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var AvbempList = (from Employeedata in _dbContext.EmployeePrimaryInfo
                                  join RequireComp in _dbContext.EmployeeAccidentInfo on Employeedata.Id equals RequireComp.EmployeeId
                                  join empj in _dbContext.EmployeeJobProfile on Employeedata.Id equals empj.EmployeeId into empjt
                                  from subempj in empjt.DefaultIfEmpty()
                                 // join loc in _dbContext.Location on subempj.LocationId equals loc.LocationId into gj
                                 // from subpet in gj.DefaultIfEmpty()
                                  where RequireComp.IsDeleted == false && RequireComp.IsActive == true && Employeedata.IsDeleted == false && Employeedata.IsActive == true && (string.IsNullOrEmpty(request.SearchTextByName) || Employeedata.FirstName.Contains(request.SearchTextByName) || Employeedata.LastName.Contains(request.SearchTextByName)) 
                                  select new
                                  {
                                      RequireComp,
                                      Id=RequireComp.Id,
                                      Employeedata.FirstName,
                                      Employeedata.MiddleName,
                                      Employeedata.LastName,
                                     // subpet.Name,
                                      IncidentType= RequireComp.EventType,
                                      IncidentDate=RequireComp.AccidentDate,
                                      ReportedTo=RequireComp.ReportedTo,
                                      FullName = Employeedata.FirstName + " " + ((Employeedata.MiddleName == null) ? "" : " " + Employeedata.MiddleName) + " " + ((Employeedata.LastName == null) ? "" : " " + Employeedata.LastName),
                                      EventTypeName = _dbContext.StandardCode.Where(x => x.ID == RequireComp.EventType).Select(x => x.CodeDescription).FirstOrDefault(),
                                      LocationName = RequireComp.OtherLocation != "" ? RequireComp.OtherLocation : _dbContext.Location.Where(x => x.LocationId == RequireComp.LocationId).Select(x => x.Name).FirstOrDefault(),
                                      ReportedToName = _dbContext.EmployeePrimaryInfo.Where(x => x.Id == RequireComp.ReportedTo).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                                      RaisedByName = _dbContext.EmployeePrimaryInfo.Where(x => x.Id == RequireComp.RaisedBy).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                                      CreatedDate = RequireComp.CreatedDate,
                                      LocationType=RequireComp.LocationType,
                                      LocationTypeName = _dbContext.StandardCode.Where(x => x.Value == RequireComp.LocationType).Select(x => x.CodeDescription).FirstOrDefault(),
                                      OtherLocation=RequireComp.OtherLocation,
                                      ActionTaken=RequireComp.ActionTaken
                                  });
                
               if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Employee.EmployeeAccidentOrderBy.Name:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.FullName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.FullName);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAccidentOrderBy.IncidentType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.EventTypeName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.EventTypeName);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAccidentOrderBy.IncidentDate:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.IncidentDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.IncidentDate);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAccidentOrderBy.LocationType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.LocationType);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.LocationType);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAccidentOrderBy.Location:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.LocationName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.LocationName);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAccidentOrderBy.ReportedTo:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.ReportedToName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.ReportedToName
                                );
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAccidentOrderBy.ActionTaken:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.ActionTaken);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.ActionTaken
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


                    //empList = empList.Skip<EmployeePrimaryInfo>((request.PageNo > 0 ? (request.PageNo - 1) : request.PageNo) * request.PageSize).Take<EmployeePrimaryInfo>(request.PageSize).ToList();
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

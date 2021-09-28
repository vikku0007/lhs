
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
using LHSAPI.Application.Employee.Models;

namespace LHSAPI.Application.Employee.Queries.GetEmployeeAccidentIncident
{
    public class GetEmployeeAccidentListQueryHandler : IRequestHandler<GetEmployeeAccidentListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeAccidentListQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
       
        public async Task<ApiResponse> Handle(GetEmployeeAccidentListQuery request, CancellationToken cancellationToken)
        {
            
            ApiResponse response = new ApiResponse();
            try
            {
                var AvbempList = (from accident in _dbContext.EmployeeAccidentInfo
                                where accident.IsActive == true && accident.IsDeleted == false && accident.EmployeeId == request.EmployeeId
                              select new LHSAPI.Application.Employee.Models.EmployeeAccidentInfoModel
                              {
                                  Id = accident.Id,
                                  EmployeeId = accident.Id,
                                  AccidentDate = accident.AccidentDate,
                                  EventType = accident.EventType,
                                  LocationId = accident.LocationId,
                                  RaisedBy = accident.RaisedBy,
                                  ReportedTo = accident.ReportedTo,
                                  BriefDescription = accident.BriefDescription,
                                  DetailedDescription = accident.DetailedDescription,
                                  EventTypeName = _dbContext.StandardCode.Where(x => x.ID == accident.EventType).Select(x => x.CodeDescription).FirstOrDefault(),
                                  LocationName = accident.OtherLocation != "" ? accident.OtherLocation : _dbContext.Location.Where(x => x.LocationId == accident.LocationId).Select(x => x.Name).FirstOrDefault(),
                                  ReportedToName = _dbContext.EmployeePrimaryInfo.Where(x => x.Id == accident.ReportedTo).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                                  RaisedByName = _dbContext.EmployeePrimaryInfo.Where(x => x.Id == accident.RaisedBy).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                                  CreatedDate = accident.CreatedDate,
                                  LocationType = accident.LocationType,
                                  LocationTypeName = _dbContext.StandardCode.Where(x => x.Value == accident.LocationType).Select(x => x.CodeDescription).FirstOrDefault(),
                                  OtherLocation = accident.OtherLocation,
                                  ActionTaken = accident.ActionTaken,
                                  IncidentTime=accident.IncidentTime,
                                  IncidentTimeName = accident.AccidentDate.Date.Add(accident.IncidentTime).ToString("hh:mm tt"),
                                  IncidentTimeTake = accident.AccidentDate.Date.Add(accident.IncidentTime).ToString("hh:mm"),
                                  
                              });
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                       
                        case Common.Enums.Employee.EmployeeAccidentinfoOrderBy.EventType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.EventType);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.EventType);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAccidentinfoOrderBy.AccidentDate:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.AccidentDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.AccidentDate);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAccidentinfoOrderBy.Locationtype:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.LocationTypeName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.LocationTypeName);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAccidentinfoOrderBy.Location:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.LocationName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.LocationName);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAccidentinfoOrderBy.RaisedBy:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.RaisedBy);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.RaisedBy
                                );
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAccidentinfoOrderBy.ReportedTo:
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
                        case Common.Enums.Employee.EmployeeAccidentinfoOrderBy.Description:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.BriefDescription);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.BriefDescription
                                );
                            }

                            break;
                        case Common.Enums.Employee.EmployeeAccidentinfoOrderBy.ActionTaken:
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
                        case Common.Enums.Employee.EmployeeAccidentinfoOrderBy.IncidentTimeName:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.IncidentTimeName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.IncidentTimeName
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
       
    }
}

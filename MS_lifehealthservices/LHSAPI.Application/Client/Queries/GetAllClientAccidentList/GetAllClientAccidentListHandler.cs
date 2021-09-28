
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

namespace LHSAPI.Application.Client.Queries.GetAllClientAccidentList
{
    public class GetAllClientAccidentListQueryHandler : IRequestHandler<GetAllClientAccidentListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetAllClientAccidentListQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
       
        public async Task<ApiResponse> Handle(GetAllClientAccidentListQuery request, CancellationToken cancellationToken)
        {
            
            ApiResponse response = new ApiResponse();
            try
            {
                var AvbempList = (from Clientdata in _dbContext.ClientPrimaryInfo
                                  join accident in _dbContext.ClientAccidentIncidentInfo on Clientdata.Id equals accident.ClientId
                                  join loc in _dbContext.Location on accident.LocationId equals loc.LocationId into gj
                                  from subpet in gj.DefaultIfEmpty()
                                  where accident.IsDeleted == false && accident.IsActive == true && Clientdata.IsDeleted == false && Clientdata.IsActive == true && (string.IsNullOrEmpty(request.SearchTextByName) || Clientdata.FirstName.Contains(request.SearchTextByName) || Clientdata.LastName.Contains(request.SearchTextByName)) && (string.IsNullOrEmpty(request.SearchTextBylocation) || (subpet != null && subpet.Name.Contains(request.SearchTextBylocation)))
                                  select new LHSAPI.Application.Client.Models.ClientAccidentIncidentModel
                                {
                                    Id = accident.Id,
                                    ClientId = accident.ClientId,
                                    EmployeeId = accident.EmployeeId,
                                    DepartmentId = accident.DepartmentId,
                                    AccidentDate = accident.AccidentDate,
                                    IncidentType = accident.IncidentType,
                                    LocationId = accident.LocationId,
                                    ReportedBy = accident.ReportedBy,
                                    FollowUp = accident.FollowUp,
                                    IncidentCause = accident.IncidentCause,
                                    PhoneNo = accident.PhoneNo,
                                    PoliceNotified = accident.PoliceNotified,
                                    EventTypeName = _dbContext.StandardCode.Where(x => x.ID == accident.IncidentType).Select(x => x.CodeDescription).FirstOrDefault(),
                                    LocationName = accident.OtherLocation != "" ? accident.OtherLocation : _dbContext.Location.Where(x => x.LocationId == accident.LocationId).Select(x => x.Name).FirstOrDefault(),
                                    DepartmentName = _dbContext.StandardCode.Where(x => x.ID == accident.DepartmentId).Select(x => x.CodeDescription).FirstOrDefault(),
                                    ReportedByName = _dbContext.EmployeePrimaryInfo.Where(x => x.Id == accident.ReportedBy).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                                    EmployeeName = _dbContext.EmployeePrimaryInfo.Where(x => x.Id == accident.EmployeeId).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                                    FullName = _dbContext.ClientPrimaryInfo.Where(x => x.Id == accident.ClientId).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                                   CreatedDate=accident.CreatedDate,
                                      LocationType = accident.LocationType,
                                      LocationTypeName = _dbContext.StandardCode.Where(x => x.Value == accident.LocationType).Select(x => x.CodeDescription).FirstOrDefault(),
                                      OtherLocation = accident.OtherLocation
                                  });

                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Client.ClientAccidentOrderBy.Name:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.FullName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.FullName);
                            }
                            break;
                        case Common.Enums.Client.ClientAccidentOrderBy.ReportedTo:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.ReportedByName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.ReportedByName);
                            }
                            break;
                        case Common.Enums.Client.ClientAccidentOrderBy.Department:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.DepartmentId);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.DepartmentId);
                            }
                            break;

                        case Common.Enums.Client.ClientAccidentOrderBy.EventType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.IncidentType);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.IncidentType);
                            }
                            break;
                        case Common.Enums.Client.ClientAccidentOrderBy.Location:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.LocationName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.LocationName
                                );
                            }
                            break;
                        case Common.Enums.Client.ClientAccidentOrderBy.AccidentDate:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.AccidentDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.AccidentDate
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

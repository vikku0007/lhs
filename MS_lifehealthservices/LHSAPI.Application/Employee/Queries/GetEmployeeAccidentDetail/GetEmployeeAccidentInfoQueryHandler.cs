
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

namespace LHSAPI.Application.Employee.Queries.GetEmployeeAccidentDetail
{
    public class GetEmployeeAccidentInfoQueryHandler : IRequestHandler<GetEmployeeAccidentInfoQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeAccidentInfoQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }
       
        public async Task<ApiResponse> Handle(GetEmployeeAccidentInfoQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

               
                var AccidentList = (from accident in _dbContext.EmployeeAccidentInfo
                                  where accident.IsActive == true && accident.IsDeleted == false && accident.Id == request.Id
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
                                      IncidentTime = accident.IncidentTime,
                                      IncidentTimeName = accident.AccidentDate.Date.Add(accident.IncidentTime).ToString("hh:mm tt"),
                                      IncidentTimeTake = accident.AccidentDate.Date.Add(accident.IncidentTime).ToString("hh:mm"),

                                  });
                if (AccidentList != null && AccidentList.Any())
                {
                    var totalCount = AccidentList.Count();
                    //LocationList = LocationList.Skip<Location>((request.PageNo - 1) * request.PageSize).Take<Location>(request.PageSize);
                    response.ResponseData = AccidentList.ToList();
                     response.StatusCode = HTTPStatusCode.SUCCESSSTATUSCODE;
                    response.Total = totalCount;
                }
                else
                {
                    response.Message = ResponseMessage.NOTFOUND;
                    response.StatusCode = HTTPStatusCode.NO_DATA_FOUND;
                }

            }
            catch (Exception ex)
            {
        response.Status = (int)Number.Zero;
        response.Message = ResponseMessage.Error;
                response.StatusCode = HTTPStatusCode.INTERNAL_SERVER_ERROR;
            }
            return response;
        }
       
    }
}

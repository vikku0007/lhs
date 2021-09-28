
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

namespace LHSAPI.Application.Client.Queries.GetClientAccidentDetails
{
    public class GetClientAccidentDetailsHandler : IRequestHandler<GetClientAccidentDetailsQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetClientAccidentDetailsHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
       
        public async Task<ApiResponse> Handle(GetClientAccidentDetailsQuery request, CancellationToken cancellationToken)
        {
            
            ApiResponse response = new ApiResponse();
            try
            {
                var commList = (from accident in _dbContext.ClientAccidentIncidentInfo
                                where accident.IsActive == true && accident.IsDeleted == false && accident.Id == request.Id
                              select new LHSAPI.Application.Client.Models.ClientAccidentIncidentModel
                              {
                                  Id = accident.Id,
                                  EmployeeId = accident.EmployeeId,
                                  DepartmentId = accident.DepartmentId,
                                  AccidentDate = accident.AccidentDate,
                                  IncidentType = accident.IncidentType,
                                  LocationId = accident.LocationId,
                                  ReportedBy = accident.ReportedBy,
                                  FollowUp = accident.FollowUp,
                                  IncidentCause=accident.IncidentCause,
                                  PhoneNo=accident.PhoneNo,
                                  PoliceNotified=accident.PoliceNotified,
                                  EventTypeName = _dbContext.StandardCode.Where(x => x.ID == accident.IncidentType).Select(x => x.CodeDescription).FirstOrDefault(),
                                  LocationName = accident.OtherLocation != "" ? accident.OtherLocation : _dbContext.Location.Where(x => x.LocationId == accident.LocationId).Select(x => x.Name).FirstOrDefault(),
                                  DepartmentName=_dbContext.StandardCode.Where(x => x.ID == accident.DepartmentId).Select(x => x.CodeDescription).FirstOrDefault(),
                                  ReportedByName = _dbContext.EmployeePrimaryInfo.Where(x => x.Id == accident.ReportedBy).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                                  EmployeeName = _dbContext.EmployeePrimaryInfo.Where(x => x.Id == accident.EmployeeId).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                               LocationType = accident.LocationType,
                                  LocationTypeName = _dbContext.StandardCode.Where(x => x.Value == accident.LocationType).Select(x => x.CodeDescription).FirstOrDefault(),
                                  OtherLocation = accident.OtherLocation
                              }).OrderByDescending(x => x.Id).ToList();

                if (commList != null && commList.Count > 0)
                {
                   
                    response.SuccessWithOutMessage(commList.ToList());
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
       
    }
}

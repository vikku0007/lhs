
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
using LHSAPI.Application.Shift.Models;

namespace LHSAPI.Application.Employee.Queries.GetEmployeeTodayShiftInfo
{
    public class GetEmployeeTodayShiftInfoHandler : IRequestHandler<GetEmployeeTodayShiftInfoQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeTodayShiftInfoHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }
       
        public async Task<ApiResponse> Handle(GetEmployeeTodayShiftInfoQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                List<ShiftInfoViewModel> list = new List<ShiftInfoViewModel>();
                var AvbempList = (from shiftdata in _dbContext.ShiftInfo
                                  join location in _dbContext.Location on shiftdata.LocationId equals location.LocationId into gj
                                  from subpet in gj.DefaultIfEmpty()
                                  join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                  join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                                  join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                  join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                                  join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                  where shiftdata.IsDeleted == false && shiftdata.IsActive == true
                                  && shiftdata.StartDate.Date == (DateTime.Now.Date)
                                  && emInfo.Id==request.EmployeeId
                                  select shiftdata
                                 ).Distinct().ToList();


                foreach (var item in AvbempList)
                {
                    ShiftInfoViewModel shift = new ShiftInfoViewModel()
                    {
                        Id = item.Id,
                        Description = item.Description,
                        ClientCount = item.ClientCount,
                        EmployeeCount = item.EmployeeCount,
                        StartDate = item.StartDate.Date.Add(item.StartTime),
                        StartTime = item.StartTime,
                        EndDate = item.EndDate.Date.Add(item.EndTime + new TimeSpan(1, 0, 0)),
                        EndTime = item.EndTime,
                        IsPublished = item.IsPublished,
                        LocationId = item.LocationId,
                        LocationType = item.LocationType,
                        StartTimeString = item.StartDate.Date.Add(item.StartTime).ToString("hh:mm tt"),
                        EndTimeString = item.EndDate.Date.Add(item.EndTime).ToString("hh:mm tt"),
                        Duration = item.Duration,
                        LocationName = item.LocationId!=null?_dbContext.Location.Where(x => x.LocationId == item.LocationId).Select(x => x.Name).FirstOrDefault():item.OtherLocation!=null?item.OtherLocation:"",
                        Reminder = item.Reminder,
                       // StatusName = _dbContext.StandardCode.Where(x => x.ID == item.StatusId).Select(x => x.CodeDescription).FirstOrDefault(),
                   
                    };
                   
                    shift.ClientShiftInfoViewModel = (from shiftdata in _dbContext.ShiftInfo
                                                      join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                                      join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                                      where shiftdata.IsDeleted == false && shiftdata.IsActive == true 
                                                         && clShift.ShiftId==shift.Id
                                                      select new ClientShiftInfoViewModel
                                                      {
                                                          Id = shiftdata.Id,
                                                          ClientId = clInfo.Id,
                                                          Name = clInfo.FirstName + " " + (clInfo.MiddleName == null ? "" : clInfo.MiddleName) + " " + clInfo.LastName,
                                                      }).OrderByDescending(x => x.Id).ToList();
                  
                    list.Add(shift);

                }

                //.Distinct().;
                if (list != null && list.Any())
                {
                    var totalCount = _dbContext.ShiftInfo.Where(x => x.IsActive && x.IsDeleted == false).Count();

                    response.Total = totalCount;
                    response.SuccessWithOutMessage(list.OrderByDescending(x => x.Id).ToList());

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

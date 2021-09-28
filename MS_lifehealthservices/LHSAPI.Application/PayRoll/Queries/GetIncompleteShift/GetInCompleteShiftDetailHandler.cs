using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using LHSAPI.Application.PayRoll.Models;
using LHSAPI.Application.Shift.Models;

namespace LHSAPI.Application.PayRoll.Queries.GetIncompleteShift
{
    public class GetInCompleteShiftDetailHandler : IRequestHandler<GetInCompleteShiftsInfoQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        public GetInCompleteShiftDetailHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse> Handle(GetInCompleteShiftsInfoQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var AvbempList = (from shiftdata in _dbContext.ShiftInfo
                                  join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                  join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                                  join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                  join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                                  join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                  join location in _dbContext.Location on shiftdata.LocationId equals location.LocationId
                                  into gj
                                  from subpet in gj.DefaultIfEmpty()
                                  join tracker in _dbContext.EmployeeShiftTracker on
                                        shiftdata.Id equals tracker.ShiftId into gj1
                                  from subset1 in gj1.DefaultIfEmpty()
                                  where shiftdata.IsDeleted == false && shiftdata.IsActive == true
                                  && emShift.IsAccepted == true && ((subset1.IsShiftCompleted == false || subset1.IsShiftCompleted == null) || subset1.IsLogin == true)
                                  && status.CodeDescription.ToLower() != "complete"
                                  && ((shiftdata.StartDate.Date <= DateTime.Now.Date && shiftdata.EndDate.Date <= DateTime.Now.Date)
                                  || (shiftdata.StartDate <= DateTime.Now.Date) || (shiftdata.EndDate.Date <= DateTime.Now.Date))
                                  && (request.SearchByEmpName == 0 || emInfo.Id == request.SearchByEmpName)
                                  && (request.SearchByClientName == 0 || clInfo.Id == request.SearchByClientName)
                                  && (request.SearchTextBylocation == 0 || subpet.LocationId == request.SearchTextBylocation)
                                  && (request.SearchTextByStatus == 0 || status.ID == request.SearchTextByStatus)
                                  && (string.IsNullOrEmpty(request.SearchTextByManualAddress) || shiftdata.OtherLocation.Contains(request.SearchTextByManualAddress))
                                  && (request.SearchByStartDate == null || shiftdata.StartDate >= Convert.ToDateTime(request.SearchByStartDate))
                                  && (request.SearchByEndDate == null || shiftdata.EndDate <= Convert.ToDateTime(request.SearchByEndDate))
                                  select new IncompleteShiftInfoModel()
                                  {
                                      Id = shiftdata.Id,
                                      Description = shiftdata.Description,
                                      ClientCount = shiftdata.ClientCount,
                                      EmployeeCount = shiftdata.EmployeeCount,
                                      StartDate = shiftdata.StartDate.Date.Add(shiftdata.StartTime),
                                      StartTime = shiftdata.StartTime,
                                      EndDate = shiftdata.EndDate.Date.Add(shiftdata.EndTime),
                                      EndTime = shiftdata.EndTime,
                                      StatusId = emShift.StatusId,
                                      IsPublished = shiftdata.IsPublished,
                                      LocationId = shiftdata.LocationId,
                                      StartTimeString = shiftdata.StartDate.Date.Add(shiftdata.StartTime).ToString(@"hh\:mm tt"),
                                      EndTimeString = shiftdata.EndDate.Date.Add(shiftdata.EndTime).ToString(@"hh\:mm tt"),
                                      Duration = shiftdata.Duration,
                                      LocationName = _dbContext.Location.Where(x => x.LocationId == shiftdata.LocationId).Select(x => x.Name).FirstOrDefault(),
                                      Reminder = shiftdata.Reminder,
                                      StatusName = _dbContext.StandardCode.Where(x => x.ID == emShift.StatusId).Select(x => x.CodeDescription).FirstOrDefault(),
                                      IsShiftCompleted = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id && x.IsActive == true && x.IsDeleted == false).Select(x => x.IsShiftCompleted).FirstOrDefault(),
                                      CheckInDate = subset1.CheckInDate.Value.Date.Add(subset1.CheckInTime.Value),
                                      CheckOutDate = subset1.CheckOutDate.Value.Date.Add(subset1.CheckOutTime.Value),
                                      CheckInTimeString = subset1.CheckInDate.Value.Date.Add(subset1.CheckInTime.Value).ToString(@"hh\:mm tt"),
                                      CheckOutTimeString = subset1.CheckOutDate.Value.Date.Add(subset1.CheckOutTime.Value).ToString(@"hh\:mm tt"),
                                      IsLogin = subset1.IsLogin,
                                      EmployeeShiftInfoViewModel = (from Empdata in _dbContext.ShiftInfo
                                                                    join emShift in _dbContext.EmployeeShiftInfo on Empdata.Id equals emShift.ShiftId
                                                                    join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                                                                    where Empdata.IsDeleted == false && Empdata.IsActive == true && ((request.SearchByEmpName == 0 && Empdata.Id == shiftdata.Id) || (request.SearchByEmpName > 0 && emInfo.Id == request.SearchByEmpName && Empdata.Id == shiftdata.Id))
                                                                    select new EmployeeShiftInfoViewModel
                                                                    {
                                                                        Id = shiftdata.Id,
                                                                        EmployeeId = emInfo.Id,
                                                                        IsSleepOver = emShift.IsSleepOver,
                                                                        Name = emInfo.FirstName + " " + (emInfo.MiddleName == null ? "" : emInfo.MiddleName) + " " + emInfo.LastName,
                                                                    }).OrderByDescending(x => x.Id).ToList(),


                                      ClientShiftInfoViewModel = (from cldata in _dbContext.ShiftInfo
                                                                  join clShift in _dbContext.ClientShiftInfo on cldata.Id equals clShift.ShiftId
                                                                  join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                                                  where cldata.IsDeleted == false && cldata.IsActive == true && ((request.SearchByClientName == 0 && cldata.Id == shiftdata.Id) || (request.SearchByClientName > 0 && clInfo.Id == request.SearchByClientName && cldata.Id == shiftdata.Id))
                                                                  select new ClientShiftInfoViewModel
                                                                  {
                                                                      Id = shiftdata.Id,
                                                                      ClientId = clInfo.Id,
                                                                      Name = clInfo.FirstName + " " + (clInfo.MiddleName == null ? "" : clInfo.MiddleName) + " " + clInfo.LastName,
                                                                  }).OrderByDescending(x => x.Id).ToList(),
                                      ServiceTypeViewModel = (from typedata in _dbContext.ShiftInfo
                                                              join shiftservice in _dbContext.ServiceTypeShiftInfo on typedata.Id equals shiftservice.ShiftId
                                                              join sc in _dbContext.ServiceDetails on shiftservice.ServiceTypeId equals sc.Id
                                                              where typedata.IsDeleted == false && typedata.IsActive == true && typedata.Id == shiftdata.Id
                                                              select new ServiceTypeViewModel
                                                              {
                                                                  Id = shiftservice.Id,
                                                                  ServiceTypeId = shiftservice.ServiceTypeId,
                                                                  Name = sc.SupportItemName,
                                                              }).ToList()

                                  }
                                 ).AsQueryable().Distinct().ToList();


                if (AvbempList != null && AvbempList.Any())
                {
                    AvbempList = CalculateCustomDuration(AvbempList);
                    var clientlist = AvbempList.OrderByDescending(x => x.Id).ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                    var totalCount = AvbempList.Count();
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(clientlist);
                }
                else
                {
                    response = response.NotFound();
                }
                //var incompleteShifts = (from shift in _dbContext.ShiftInfo
                //                        join empShift in _dbContext.EmployeeShiftInfo on
                //                        shift.Id equals empShift.ShiftId
                //                        join empPrimary in _dbContext.EmployeePrimaryInfo on
                //                        empShift.EmployeeId equals empPrimary.Id
                //                        join tracker in _dbContext.EmployeeShiftTracker on
                //                        shift.Id equals tracker.ShiftId into gj
                //                        from subset in gj.DefaultIfEmpty()
                //                        where shift.IsActive == true && shift.IsDeleted == false && empShift.IsActive == true && empShift.IsDeleted == false
                //                        && empShift.IsAccepted == true && (subset.IsShiftCompleted == false || subset.IsLogin == true)
                //                        select new IncompleteShiftInfoModel
                //                        {
                //                            ShiftId = shift.Id,
                //                            FullName = empPrimary.FirstName + " " + empPrimary.LastName,
                //                            EmployeeId = empPrimary.Id,
                //                            StartDate = shift.StartDate.Date,
                //                            EndDate = shift.EndDate.Date,
                //                            StartTime = shift.StartTime,
                //                            EndTime = shift.EndTime,
                //                            Description = shift.Description,
                //                            Duration = shift.Duration,
                //                            StartTimeString = shift.StartDate.Date.Add(shift.StartTime).ToString(@"hh\:mm tt"),
                //                            EndTimeString = shift.EndDate.Date.Add(shift.EndTime).ToString(@"hh\:mm tt")
                //                        }).ToList();
                //response.SuccessWithOutMessage(incompleteShifts);
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }
        private List<IncompleteShiftInfoModel> CalculateCustomDuration(List<IncompleteShiftInfoModel> shiftdetails)
        {
            string temp = null;
            try
            {
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                foreach (var shiftdetail in shiftdetails)
                {
                    if (shiftdetail.IsActiveNight == true)
                    {
                        startDate = shiftdetail.StartDate.Date.AddHours(24);
                        endDate = shiftdetail.EndDate.Date.AddHours(6);
                        double normalHours = 0;
                        double activeHours = 0;
                        if (shiftdetail.StartDate <= startDate && shiftdetail.EndDate >= endDate)
                        {
                            normalHours = shiftdetail.Duration - 6;
                            activeHours = 6;
                        }
                        else if (shiftdetail.StartDate >= startDate && shiftdetail.EndDate <= endDate)
                        {
                            normalHours = 0;
                            activeHours = shiftdetail.Duration;
                        }
                        else if (shiftdetail.StartDate >= startDate && shiftdetail.EndDate >= endDate)
                        {
                            activeHours = (endDate - shiftdetail.StartDate).TotalHours;
                            normalHours = shiftdetail.Duration - activeHours;
                        }
                        else if (shiftdetail.StartDate <= startDate && shiftdetail.EndDate <= endDate)
                        {
                            activeHours = (shiftdetail.EndDate - startDate).TotalHours;
                            normalHours = shiftdetail.Duration - activeHours;
                        }

                        temp = "Normal Hours : " + Math.Round(normalHours, 2) + " Hrs and Active Hours : " + activeHours + " Hrs";
                    }
                    else if (shiftdetail.IsSleepOver == true)
                    {
                        startDate = shiftdetail.StartDate.Date.AddHours(22);
                        endDate = shiftdetail.EndDate.Date.AddHours(6);
                        double normalHours = 0;
                        double sleepHours = 0;
                        if (shiftdetail.StartDate <= startDate && shiftdetail.EndDate >= endDate)
                        {
                            normalHours = shiftdetail.Duration - 8;
                            sleepHours = 1;
                        }
                        else if (shiftdetail.StartDate >= startDate && shiftdetail.EndDate <= endDate)
                        {
                            normalHours = 0;
                            sleepHours = 1;
                        }
                        else if (shiftdetail.StartDate >= startDate && shiftdetail.EndDate >= endDate)
                        {
                            sleepHours = (endDate - shiftdetail.StartDate).TotalHours;
                            normalHours = shiftdetail.Duration - sleepHours;
                            sleepHours = 1;
                        }
                        else if (shiftdetail.StartDate <= startDate && shiftdetail.EndDate <= endDate)
                        {
                            sleepHours = (shiftdetail.EndDate - startDate).TotalHours;
                            normalHours = shiftdetail.Duration - sleepHours;
                            sleepHours = 1;
                        }

                        temp = "Normal Hours : " + Math.Round(normalHours, 2) + " Hrs and Sleep Over : " + sleepHours;
                    }
                    else
                    {
                        double normalHours = shiftdetail.Duration;
                        temp = "Normal Hours : " + Math.Round(normalHours, 2) + " Hrs";
                    }
                    shiftdetail.CustomDuration = temp;
                }
            }
            catch (Exception ex)
            {
            }
            return shiftdetails;
        }
    }
}

using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LHSAPI.Domain.Entities;
using LHSAPI.Application.Shift.Models;

namespace LHSAPI.Application.Shift.Queries.GetShiftList
{
    public class GetShiftListHandler : IRequestHandler<GetAllShiftListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetShiftListHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
        #region Get Shift List

        public async Task<ApiResponse> Handle(GetAllShiftListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                List<ShiftInfoViewModel> list = new List<ShiftInfoViewModel>();
                var AvbempList = (from shiftdata in _dbContext.ShiftInfo
                                  join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                  join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                                  join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                  join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                                  join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                  join location in _dbContext.Location on shiftdata.LocationId equals location.LocationId
                                  into gj
                                  from subpet in gj.DefaultIfEmpty()
                                  where shiftdata.IsDeleted == false && shiftdata.IsActive == true
                                  && (request.SearchByEmpName == 0 || emInfo.Id == request.SearchByEmpName)
                                  && (request.SearchByClientName == 0 || clInfo.Id == request.SearchByClientName)
                                  && (request.SearchTextBylocation == 0 || subpet.LocationId == request.SearchTextBylocation)
                                  && (request.SearchTextByStatus == 0 || status.ID == request.SearchTextByStatus)
                                  && (string.IsNullOrEmpty(request.SearchTextByManualAddress) || shiftdata.OtherLocation.Contains(request.SearchTextByManualAddress))
                                  && (request.SearchByStartDate == null || shiftdata.StartDate >= Convert.ToDateTime(request.SearchByStartDate))
                                  && (request.SearchByEndDate == null || shiftdata.EndDate <= Convert.ToDateTime(request.SearchByEndDate))
                                  select new ShiftInfoViewModel()
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
                                      IsShiftCompleted = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id && x.IsActive==true && x.IsDeleted==false).Select(x=>x.IsShiftCompleted).FirstOrDefault(),
                                      IsActiveNight=emShift.IsActiveNight,
                                      IsSleepOver=emShift.IsSleepOver,
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
                //foreach (var item in AvbempList)
                //{
                //  ShiftInfoViewModel shift = new ShiftInfoViewModel()
                //  {
                //    Id = item.Id,
                //    Description = item.Description,
                //    ClientCount = item.ClientCount,
                //    EmployeeCount = item.EmployeeCount,
                //    StartDate = item.StartDate.Date.Add(item.StartTime),
                //    StartTime = item.StartTime,
                //    EndDate = item.EndDate.Date.Add(item.EndTime),
                //    EndTime = item.EndTime,
                //    StatusId = item.StatusId,
                //    IsPublished = item.IsPublished,
                //    LocationId = item.LocationId,
                //    StartTimeString = item.StartDate.Date.Add(item.StartTime).ToString(@"hh\:mm tt"),
                //    EndTimeString = item.EndDate.Date.Add(item.EndTime).ToString(@"hh\:mm tt"),
                //    Duration = item.Duration,
                //    LocationName = _dbContext.Location.Where(x => x.LocationId == item.LocationId).Select(x => x.Name).FirstOrDefault(),
                //    Reminder = item.Reminder,        
                //    StatusName = _dbContext.StandardCode.Where(x => x.ID == item.StatusId).Select(x => x.CodeDescription).FirstOrDefault(),
                //  };
                //  shift.EmployeeShiftInfoViewModel = (from shiftdata in _dbContext.ShiftInfo
                //                                     join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                //                                     join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id

                //                                     where shiftdata.IsDeleted == false && shiftdata.IsActive == true && ((request.SearchByEmpName == 0 && shiftdata.Id == item.Id) || (request.SearchByEmpName > 0 && emInfo.Id == request.SearchByEmpName && shiftdata.Id == item.Id))
                //                                      select new EmployeeShiftInfoViewModel
                //                                     {
                //                                       Id = shiftdata.Id,
                //                                       EmployeeId = emInfo.Id,
                //                                       IsSleepOver = emShift.IsSleepOver,
                //                                       Name = emInfo.FirstName + " " + (emInfo.MiddleName == null ? "" : emInfo.MiddleName) + " " + emInfo.LastName,
                //                                     }).OrderByDescending(x => x.Id).ToList();


                //  shift.ClientShiftInfoViewModel = (from shiftdata in _dbContext.ShiftInfo
                //                                   join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                //                                   join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                //                                   where shiftdata.IsDeleted == false && shiftdata.IsActive == true && ((request.SearchByClientName == 0 && shiftdata.Id == item.Id) || (request.SearchByClientName > 0 && clInfo.Id == request.SearchByClientName && shiftdata.Id == item.Id))
                //                                    select new ClientShiftInfoViewModel
                //                                   {
                //                                     Id = shiftdata.Id,
                //                                     ClientId = clInfo.Id,
                //                                     Name = clInfo.FirstName + " " + (clInfo.MiddleName == null ? "" : clInfo.MiddleName) + " " + clInfo.LastName,
                //                                   }).OrderByDescending(x => x.Id).ToList();
                //  shift.ServiceTypeViewModel = (from shiftdata in _dbContext.ShiftInfo
                //                                join shiftservice in _dbContext.ServiceTypeShiftInfo on shiftdata.Id equals shiftservice.ShiftId
                //                                join sc in _dbContext.ServiceDetails on shiftservice.ServiceTypeId equals sc.Id
                //                                where shiftdata.IsDeleted == false && shiftdata.IsActive == true && shiftdata.Id == item.Id
                //                                select new ServiceTypeViewModel
                //                                {
                //                                  Id = shiftservice.Id,
                //                                  ServiceTypeId = shiftservice.ServiceTypeId,
                //                                  Name = sc.SupportItemName,
                //                                }).ToList();

                //  list.Add(shift);

                //}



            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }
        #endregion

        private List<ShiftInfoViewModel> CalculateCustomDuration(List<ShiftInfoViewModel> shiftdetails)
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

using LHSAPI.Application.Interface;
using LHSAPI.Application.Shift.Commands.Create.AddShiftInfo;
using LHSAPI.Application.Shift.Commands.Update.UpdateShiftInfo;
using LHSAPI.Application.Shift.Models;
using LHSAPI.Application.Shift.Queries.GetEmployeeViewCalendar;
using LHSAPI.Application.Shift.Queries.GetShiftPopOverInfo;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Common.CommonMethods;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Services
{
    public class ShiftService : IShiftService
    {
        private readonly LHSDbContext _dbContext;
        private readonly ISessionService _ISessionService;
        public ShiftService(LHSDbContext dbContext, ISessionService ISessionService)
        {
            _dbContext = dbContext;
            _ISessionService = ISessionService;
        }

        public ShiftInfoViewModel GetShiftDetail(int shiftId, bool isshiftCompleted = false)
        {
            var _shift = (from shiftdata in _dbContext.ShiftInfo
                          join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                          join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                          // join emShift in _dbContext.EmployeeShiftTracker on shiftdata.Id equals emShift.ShiftId into gj
                          //from subpet in gj.DefaultIfEmpty()
                          where shiftdata.IsDeleted == false && shiftdata.IsActive == true && shiftdata.Id == shiftId //&& subpet.IsShiftCompleted == isshiftCompleted
                          select new ShiftInfoViewModel()
                          {
                              Id = shiftdata.Id,
                              Description = shiftdata.Description,
                              ClientCount = shiftdata.ClientCount,
                              EmployeeCount = shiftdata.EmployeeCount,
                              StartDate = shiftdata.StartDate.Date.Add(shiftdata.StartTime),
                              StartTime = shiftdata.StartTime,
                              StartTimeString = shiftdata.StartDate.Date.Add(shiftdata.StartTime).ToString(@"hh\:mm tt"),
                              EndTimeString = shiftdata.EndDate.Date.Add(shiftdata.EndTime).ToString(@"hh\:mm tt"),
                              Duration = shiftdata.Duration,
                              EndDate = shiftdata.EndDate.Date.Add(shiftdata.EndTime),
                              EndTime = shiftdata.EndTime,
                              StatusId = emShift.StatusId,
                              IsPublished = shiftdata.IsPublished,
                              LocationId = shiftdata.LocationId,
                              OtherLocation = shiftdata.OtherLocation,
                              LocationType = shiftdata.LocationType,
                              LocationName = _dbContext.Location.Where(x => x.LocationId == shiftdata.LocationId).Select(x => x.Name).FirstOrDefault(),
                              Reminder = shiftdata.Reminder,
                              StatusName = _dbContext.StandardCode.Where(x => x.ID == emShift.StatusId).Select(x => x.CodeDescription).FirstOrDefault(),
                              IsDeleted = shiftdata.IsDeleted,
                              ShiftRepeatType = shiftdata.ShiftRepeatType,
                              IsSleepOver = emShift.IsSleepOver,
                              IsActiveNight = emShift.IsActiveNight,
                              Remark = shiftdata.Remark,
                              AdminCheckoutRemark = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id).Select(x => x.AdminCheckOutRemark).FirstOrDefault(),
                              CheckoutRemark = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id).Select(x => x.CheckOutRemarks).FirstOrDefault(),
                          }
                          ).FirstOrDefault();

            if (_shift != null)
            {
                ShiftInfoViewModel shift = new ShiftInfoViewModel()
                {
                    Id = _shift.Id,
                    Description = _shift.Description,
                    ClientCount = _shift.ClientCount,
                    EmployeeCount = _shift.EmployeeCount,
                    StartDate = _shift.StartDate.Date.Add(_shift.StartTime),
                    StartTime = _shift.StartTime,
                    StartTimeString = _shift.StartDate.Date.Add(_shift.StartTime).ToString(@"hh\:mm tt"),
                    EndTimeString = _shift.EndDate.Date.Add(_shift.EndTime).ToString(@"hh\:mm tt"),
                    Duration = _shift.Duration,
                    EndDate = _shift.EndDate.Date.Add(_shift.EndTime),
                    EndTime = _shift.EndTime,
                    StatusId = _shift.StatusId,
                    IsPublished = _shift.IsPublished,
                    LocationId = _shift.LocationId,
                    OtherLocation = _shift.OtherLocation,
                    LocationType = _shift.LocationType,
                    LocationName = _dbContext.Location.Where(x => x.LocationId == _shift.LocationId).Select(x => x.Name).FirstOrDefault(),
                    Reminder = _shift.Reminder,
                    StatusName = _shift.StatusName,
                    IsDeleted = _shift.IsDeleted,
                    ShiftRepeatType = _shift.ShiftRepeatType,
                    IsActiveNight = _shift.IsActiveNight,
                    IsSleepOver = _shift.IsSleepOver,
                    Remark = _shift.Remark,
                    AdminCheckoutRemark = _shift.AdminCheckoutRemark,
                    CustomDuration = CalculateCustomDuration(_shift),
                    CheckoutRemark = _shift.CheckoutRemark
                };
                shift.EmployeeShiftInfoViewModel = (from shiftdata in _dbContext.ShiftInfo
                                                    join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                                    join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                                                    where shiftdata.IsDeleted == false && shiftdata.IsActive == true && shiftdata.Id == _shift.Id
                                                    select new EmployeeShiftInfoViewModel
                                                    {
                                                        Id = emShift.Id,
                                                        EmployeeId = emInfo.Id,
                                                        IsSleepOver = emShift.IsSleepOver,
                                                        IsActiveNight = emShift.IsActiveNight,
                                                        IsHoliday = emShift.IsHoliday,
                                                        StatusId = emShift.StatusId.Value,
                                                        StatusName = _dbContext.StandardCode.Where(x => x.ID == emShift.StatusId.Value).Select(x => x.CodeDescription).FirstOrDefault(),
                                                        Name = emInfo.FirstName + " " + (emInfo.MiddleName == null ? "" : emInfo.MiddleName) + " " + emInfo.LastName,
                                                    }).OrderByDescending(x => x.Id).ToList();


                shift.ClientShiftInfoViewModel = (from shiftdata in _dbContext.ShiftInfo
                                                  join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                                  join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                                  where shiftdata.IsDeleted == false && shiftdata.IsActive == true && shiftdata.Id == _shift.Id
                                                  select new ClientShiftInfoViewModel
                                                  {
                                                      Id = clShift.Id,
                                                      ClientId = clInfo.Id,
                                                      Name = clInfo.FirstName + " " + (clInfo.MiddleName == null ? "" : clInfo.MiddleName) + " " + clInfo.LastName,
                                                  }).OrderByDescending(x => x.Id).ToList();
                shift.ServiceTypeViewModel = (from shiftdata in _dbContext.ShiftInfo
                                              join shiftservice in _dbContext.ServiceTypeShiftInfo on shiftdata.Id equals shiftservice.ShiftId
                                              join sc in _dbContext.ServiceDetails on shiftservice.ServiceTypeId equals sc.Id
                                              where shiftdata.IsDeleted == false && shiftdata.IsActive == true && shiftdata.Id == _shift.Id
                                              select new ServiceTypeViewModel
                                              {
                                                  Id = shiftservice.Id,
                                                  ServiceTypeId = shiftservice.ServiceTypeId,
                                                  Name = sc.SupportItemName,
                                              }).ToList();
                return shift;
            }
            else
            {
                return null;
            }
        }

        private string CalculateCustomDuration(ShiftInfoViewModel shiftdetail)
        {
            string temp = null;
            try
            {
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
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

                    temp = "Normal Hours : " + normalHours + " Hrs and Active Hours : " + activeHours + " Hrs";
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

                    temp = "Normal Hours : " + normalHours + " Hrs and Sleep Over : " + sleepHours;
                }
                else
                {
                    double normalHours = shiftdetail.Duration;
                    temp = "Normal Hours : " + normalHours + " Hrs";
                }
            }
            catch (Exception ex)
            {
            }
            return temp;
        }
        public IList<ShiftToDoViewModel> GetShiftToDoList(int shiftId)
        {
            return _dbContext.ShiftToDo.Where(x => x.ShiftId == shiftId && x.IsActive && x.IsDeleted == false).Select(x => new ShiftToDoViewModel
            {
                Id = x.Id,
                ShiftId = x.ShiftId,
                Description = x.Description,
                EmployeeId = x.EmployeeId

            }).ToList();

        }
        public ApiResponse GetShiftTemplateList()
        {
            ApiResponse response = new ApiResponse();
            var list = _dbContext.ShiftTemplate.Where(x => x.IsActive && x.IsDeleted == false).Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            if (list != null)
            {
                response.Total = list.Count();
                response.SuccessWithOutMessage(list);
            }
            else
            {
                response.NotFound();
            }
            return response;

        }

        public async Task<ApiResponse> UpdateShiftStatus(int shiftId, int statusId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (shiftId > 0)
                {
                    var ExistShift = _dbContext.ShiftInfo.FirstOrDefault(x => x.Id == shiftId && x.IsDeleted == false && x.IsActive);
                    if (ExistShift != null && ExistShift.Id > 0)
                    {
                        var employee = _dbContext.EmployeeShiftInfo.Where(x => x.ShiftId == shiftId);
                        foreach (var item in employee)
                        {
                            item.StatusId = statusId;
                            item.UpdatedDate = DateTime.Now;
                            item.UpdateById = await _ISessionService.GetUserId();
                        }



                        //ExistShift.StatusId = statusId;
                        ExistShift.UpdatedDate = DateTime.Now;
                        ExistShift.UpdateById = await _ISessionService.GetUserId();
                        _dbContext.ShiftInfo.Update(ExistShift);
                        _dbContext.UpdateRange(employee);
                        await _dbContext.SaveChangesAsync();
                        response = response.Success(ExistShift);
                    }
                    else
                    {
                        response.NotFound();
                    }

                }
                else
                {
                    response.ValidationError();
                }
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;

        }

        public ApiResponse GetEmployeeViewCalendar(GetEmployeeViewCalendarQuery request, bool IsShiftCompleted = false)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                List<ShiftInfoViewModel> list = new List<ShiftInfoViewModel>();
                list = (from shiftdata in _dbContext.ShiftInfo
                        join location in _dbContext.Location on shiftdata.LocationId equals location.LocationId into gj
                        from subpet in gj.DefaultIfEmpty()
                        join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                        join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                        join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                        join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                        join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                        // join emShiftTracker in _dbContext.EmployeeShiftTracker on shiftdata.Id equals emShiftTracker.ShiftId into gjz
                        //from tracker in gjz.DefaultIfEmpty()
                        where shiftdata.IsDeleted == false && shiftdata.IsActive == true &&
                        ((shiftdata.StartDate.Date >= request.StartDate.Date && shiftdata.StartDate.Date <= request.EndDate.Date) ||
                        (shiftdata.EndDate.Date >= request.StartDate.Date && shiftdata.EndDate.Date <= request.EndDate.Date) ||
                        (shiftdata.StartDate.Date >= request.StartDate.Date && shiftdata.EndDate.Date <= request.EndDate.Date)
                        //|| (request.StartDate.Date == request.EndDate.Date && (shiftdata.StartDate.Date >= request.StartDate.Date || request.StartDate.Date <= shiftdata.EndDate.Date))
                        )
                        && (request.SearchByEmpName == 0 || emInfo.Id == request.SearchByEmpName)
                        && (request.SearchByClientName == 0 || clInfo.Id == request.SearchByClientName)
                        && (request.SearchTextBylocation == 0 || subpet.LocationId == request.SearchTextBylocation)
                        && (request.SearchTextByStatus == 0 || status.ID == request.SearchTextByStatus)
                        && (string.IsNullOrEmpty(request.SearchTextByManualAddress) || shiftdata.OtherLocation.Contains(request.SearchTextByManualAddress))
                        //&& (tracker == null ||  tracker.IsShiftCompleted == IsShiftCompleted)

                        select new ShiftInfoViewModel
                        {
                            Id = shiftdata.Id,
                            Description = shiftdata.Description,
                            ClientCount = shiftdata.ClientCount,
                            EmployeeCount = shiftdata.EmployeeCount,
                            StartDate = shiftdata.StartDate.Date.Add(shiftdata.StartTime),
                            StartTime = shiftdata.StartTime,
                            EndDate = shiftdata.EndDate.Date.Add(shiftdata.EndTime + new TimeSpan(1, 0, 0)),
                            EndTime = shiftdata.EndTime,
                            //StatusId = item.shiftdata.StatusId,
                            IsPublished = shiftdata.IsPublished,
                            LocationId = shiftdata.LocationId,
                            LocationType = shiftdata.LocationType,
                            StartTimeString = shiftdata.StartDate.Date.Add(shiftdata.StartTime).ToString("hh:mm tt"),
                            EndTimeString = shiftdata.EndDate.Date.Add(shiftdata.EndTime).ToString("hh:mm tt"),
                            Duration = shiftdata.Duration,
                            LocationName = shiftdata.LocationId.HasValue && shiftdata.LocationId.Value > 0 ? subpet.Name : shiftdata.OtherLocation,
                            Reminder = shiftdata.Reminder,
                            EmployeeId = emShift.EmployeeId,
                            IsSleepOver = emShift.IsSleepOver,
                            IsActiveNight = emShift.IsActiveNight,
                            StatusId = emShift.StatusId,
                            StatusName = status.CodeDescription,
                            IsShiftCompleted = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id && x.IsActive == true && x.IsDeleted == false).Select(x => x.IsShiftCompleted).FirstOrDefault(),
                            Name = emInfo.FirstName + " " + ((emInfo.MiddleName == null) ? "" : " " + emInfo.MiddleName) + " " + ((emInfo.LastName == null) ? "" : " " + emInfo.LastName)
                        }
                                 ).Distinct().ToList();


                //foreach (var item in AvbempList)
                //{
                //  ShiftInfoViewModel shift = new ShiftInfoViewModel()
                //  {
                //    Id = item.shiftdata.Id,
                //    Description = item.shiftdata.Description,
                //    ClientCount = item.shiftdata.ClientCount,
                //    EmployeeCount = item.shiftdata.EmployeeCount,
                //    StartDate = item.shiftdata.StartDate.Date.Add(item.shiftdata.StartTime),
                //    StartTime = item.shiftdata.StartTime,
                //    EndDate = item.shiftdata.EndDate.Date.Add(item.shiftdata.EndTime + new TimeSpan(1, 0, 0)),
                //    EndTime = item.shiftdata.EndTime,
                //    //StatusId = item.shiftdata.StatusId,
                //    IsPublished = item.shiftdata.IsPublished,
                //    LocationId = item.shiftdata.LocationId,
                //    LocationType = item.shiftdata.LocationType,
                //    StartTimeString = item.shiftdata.StartDate.Date.Add(item.shiftdata.StartTime).ToString("hh:mm tt"),
                //    EndTimeString = item.shiftdata.EndDate.Date.Add(item.shiftdata.EndTime).ToString("hh:mm tt"),
                //    Duration = item.shiftdata.Duration,
                //    LocationName = item.shiftdata.LocationId > 0 ? _dbContext.Location.Where(x => x.LocationId == item.shiftdata.LocationId).Select(x => x.Name).FirstOrDefault() : item.shiftdata.OtherLocation,
                //    Reminder = item.shiftdata.Reminder,
                //    // StatusName = item.CodeDescription,
                //  };
                //  shift.EmployeeShiftInfoViewModel = (from shiftdata in _dbContext.ShiftInfo
                //                                      join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                //                                      join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id

                //                                      where shiftdata.IsDeleted == false && shiftdata.IsActive == true && ((request.SearchByEmpName == 0 && shiftdata.Id == item.shiftdata.Id) || (emInfo.Id == request.SearchByEmpName && shiftdata.Id == item.shiftdata.Id))
                //                                      select new EmployeeShiftInfoViewModel
                //                                      {
                //                                        Id = emShift.Id,
                //                                        EmployeeId = emInfo.Id,
                //                                        IsSleepOver = emShift.IsSleepOver,
                //                                        StatusId = emShift.StatusId.Value,
                //                                        StatusName = _dbContext.StandardCode.Where(x => x.ID == emShift.StatusId.Value).Select(x => x.CodeDescription).FirstOrDefault(),
                //                                        Name = emInfo.FirstName + " " + (emInfo.MiddleName == null ? "" : emInfo.MiddleName) + " " + emInfo.LastName,
                //                                      }).ToList();


                //  shift.ClientShiftInfoViewModel = (from shiftdata in _dbContext.ShiftInfo
                //                                    join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                //                                    join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                //                                    where shiftdata.IsDeleted == false && shiftdata.IsActive == true && ((request.SearchByClientName == 0 && shiftdata.Id == item.shiftdata.Id) || (clInfo.Id == request.SearchByClientName && shiftdata.Id == item.shiftdata.Id))
                //                                    select new ClientShiftInfoViewModel
                //                                    {
                //                                      //Id = shiftdata.Id,
                //                                      ClientId = clInfo.Id,
                //                                      Name = clInfo.FirstName + " " + (clInfo.MiddleName == null ? "" : clInfo.MiddleName) + " " + clInfo.LastName,
                //                                    }).ToList();

                //  shift.ServiceTypeViewModel = (from shiftdata in _dbContext.ShiftInfo
                //                                join shiftservice in _dbContext.ServiceTypeShiftInfo on shiftdata.Id equals shiftservice.ShiftId
                //                                join sc in _dbContext.ServiceDetails on shiftservice.ServiceTypeId equals sc.Id
                //                                where shiftdata.IsDeleted == false && shiftdata.IsActive == true && shiftdata.Id == item.shiftdata.Id
                //                                select new ServiceTypeViewModel
                //                                {
                //                                  Id = shiftservice.Id,
                //                                  ServiceTypeId = shiftservice.ServiceTypeId,
                //                                  Name = sc.SupportItemName,
                //                                }).ToList();

                //  list.Add(shift);

                //}

                //.Distinct().;
                if (list != null && list.Any())
                {
                    list = CalculateCustomDuration(list);
                    response.Total = list.Count;
                    response.SuccessWithOutMessage(list);

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

        public async Task<ApiResponse> GetShiftTemplate(DateTime StartDate, DateTime EndDate, int templateId)
        {
            ApiResponse response = new ApiResponse();
            List<ShiftResponse> shiftResponseList = new List<ShiftResponse>();
            string Message = string.Empty;
            try
            {
                List<ShiftInfoViewModel> list = new List<ShiftInfoViewModel>();
                var AvbempList = _dbContext.ShiftTemplate.Where(x => x.Id == templateId).FirstOrDefault();
                if (AvbempList != null)
                {
                    var shiftTemplateList = (from template in _dbContext.ShiftInfoTemplate
                                             join shiftInfo in _dbContext.ShiftInfo on template.ShiftId equals shiftInfo.Id
                                             where template.ShiftTemplateId == AvbempList.Id
                                             select new
                                             {
                                                 shiftInfo
                                             }).ToList();
                    var Dates = CommonFunction.getDates(StartDate, EndDate);
                    foreach (var item in shiftTemplateList)
                    {

                        var hasDay = Dates.Where(x => x.DayOfWeek == item.shiftInfo.StartDate.DayOfWeek).FirstOrDefault();
                        if (hasDay != null)
                        {
                            // var shiftExist = _dbContext.ShiftInfo.Where(x => x.StartDate.Date == hasDay.Date && x.StartTime == item.StartTime && x.Description == item.Description && x.IsActive && x.IsDeleted == false).FirstOrDefault();
                            //int hasEmployee = 0;
                            //if(shiftExist != null)
                            //{

                            //}
                            if (true)
                            {
                                bool IsDateDiff = item.shiftInfo.EndDate.Subtract(item.shiftInfo.StartDate.Date).Days > 0 ? true : false;



                                var EmployeeShiftInfoViewModel = (from shiftdata in _dbContext.ShiftInfo
                                                                  join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                                                  join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                                                                  where shiftdata.Id == item.shiftInfo.Id
                                                                  select new EmployeeShiftInfoViewModel
                                                                  {
                                                                      Id = shiftdata.Id,
                                                                      EmployeeId = emInfo.Id,
                                                                      IsSleepOver = emShift.IsSleepOver,
                                                                      StatusId = emShift.StatusId.Value,
                                                                      StatusName = _dbContext.StandardCode.Where(x => x.ID == emShift.StatusId.Value).Select(x => x.CodeDescription).FirstOrDefault(),
                                                                      Name = emInfo.FirstName + " " + (emInfo.MiddleName == null ? "" : emInfo.MiddleName) + " " + emInfo.LastName,
                                                                  }).ToList();


                                //check employee shift exist or over loaded
                                var startTime = item.shiftInfo.StartTime;
                                var endTime = item.shiftInfo.EndTime;
                                var shiftEndDate = IsDateDiff ? hasDay.Date.AddDays(1) : hasDay.Date;

                                var ExistShift = CheckShiftExist(hasDay.Date, shiftEndDate, startTime, endTime, EmployeeShiftInfoViewModel.Select(x => x.EmployeeId).ToArray(), shiftResponseList);
                                if (ExistShift)
                                {
                                    continue;
                                }
                                //ExistShift = IsShiftOverLoaded(hasDay.Date, shiftEndDate, startTime, endTime, EmployeeShiftInfoViewModel.Select(x => x.EmployeeId).ToArray(), shiftResponseList);
                                //if (ExistShift)
                                //{
                                //    continue;
                                //}
                                ExistShift = IsEmployeeOnLeave(hasDay.Date, shiftEndDate, EmployeeShiftInfoViewModel.Select(x => x.EmployeeId).ToList(), shiftResponseList);
                                if (ExistShift)
                                {
                                    continue;
                                }


                                var ClientShiftInfoViewModel = (from shiftdata in _dbContext.ShiftInfo
                                                                join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                                                join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                                                where shiftdata.Id == item.shiftInfo.Id
                                                                select new ClientShiftInfoViewModel
                                                                {
                                                                    Id = shiftdata.Id,
                                                                    ClientId = clInfo.Id,
                                                                    Name = clInfo.FirstName + " " + (clInfo.MiddleName == null ? "" : clInfo.MiddleName) + " " + clInfo.LastName,
                                                                }).ToList();
                                var ServiceTypeViewModel = (from shiftdata in _dbContext.ShiftInfo
                                                            join shiftservice in _dbContext.ServiceTypeShiftInfo on shiftdata.Id equals shiftservice.ShiftId
                                                            join sc in _dbContext.ServiceDetails on shiftservice.ServiceTypeId equals sc.Id
                                                            where shiftdata.Id == item.shiftInfo.Id
                                                            select new ServiceTypeViewModel
                                                            {
                                                                Id = shiftservice.Id,
                                                                ServiceTypeId = shiftservice.ServiceTypeId,
                                                                Name = sc.SupportItemName,
                                                            }).ToList();

                                ShiftInfo _ShiftInfo = new ShiftInfo();
                                _ShiftInfo.Description = item.shiftInfo.Description;
                                _ShiftInfo.EmployeeCount = EmployeeShiftInfoViewModel.Count();
                                _ShiftInfo.ClientCount = ClientShiftInfoViewModel.Count();
                                _ShiftInfo.StartDate = hasDay.Date;
                                _ShiftInfo.StartTime = item.shiftInfo.StartTime;
                                //_ShiftInfo.EndDate = hasDay.Date;
                                _ShiftInfo.EndDate = shiftEndDate;
                                _ShiftInfo.EndTime = item.shiftInfo.EndTime;
                                _ShiftInfo.StartUtcDate = _ShiftInfo.StartDate.Date.Add(item.shiftInfo.StartTime).ToUniversalTime();
                                _ShiftInfo.EndUtcDate = _ShiftInfo.EndDate.Date.Add(item.shiftInfo.EndTime).ToUniversalTime();
                                _ShiftInfo.IsPublished = false;
                                _ShiftInfo.LocationId = item.shiftInfo.LocationId;
                                _ShiftInfo.OtherLocation = item.shiftInfo.OtherLocation;
                                //  _ShiftInfo.StatusId = item.StatusId;
                                _ShiftInfo.Reminder = item.shiftInfo.Reminder;
                                _ShiftInfo.IsDeleted = false;
                                _ShiftInfo.CreatedDate = DateTime.Now;
                                _ShiftInfo.IsActive = true;
                                _ShiftInfo.CreatedById = await _ISessionService.GetUserId();
                                _ShiftInfo.LocationType = item.shiftInfo.LocationType;
                                _ShiftInfo.Duration = _ShiftInfo.EndDate.Date.Add(_ShiftInfo.EndTime).Subtract(_ShiftInfo.StartDate.Date.Add(_ShiftInfo.StartTime)).TotalHours;
                                await _dbContext.Database.BeginTransactionAsync();

                                await _dbContext.ShiftInfo.AddAsync(_ShiftInfo);
                                _dbContext.SaveChanges();

                                foreach (var id in EmployeeShiftInfoViewModel)
                                {
                                    EmployeeShiftInfo _empShift = new EmployeeShiftInfo();
                                    _empShift.EmployeeId = id.EmployeeId;
                                    _empShift.IsSleepOver = id.IsSleepOver;
                                    _empShift.ShiftId = _ShiftInfo.Id;
                                    _empShift.IsDeleted = false;
                                    _empShift.StatusId = _dbContext.StandardCode.Where(x => x.CodeData == "ShiftStatus" && x.CodeDescription == "Pending").FirstOrDefault().ID;
                                    _empShift.IsActiveNight = id.IsSleepOver ? false : IsActiveNight(_ShiftInfo.StartDate.Date.Add(_ShiftInfo.StartTime), _ShiftInfo.EndDate.Date.Add(_ShiftInfo.EndTime));
                                    _empShift.CreatedDate = DateTime.Now;
                                    _empShift.IsHoliday = id.IsSleepOver ? false : IsHoliday(_ShiftInfo.StartDate.Date.Add(_ShiftInfo.StartTime), _ShiftInfo.EndDate.Date.Add(_ShiftInfo.EndTime));
                                    _empShift.IsActive = true;
                                    _empShift.CreatedById = await _ISessionService.GetUserId();
                                    await _dbContext.EmployeeShiftInfo.AddAsync(_empShift);
                                    _dbContext.SaveChanges();
                                    id.Id = _empShift.Id;
                                    id.IsActiveNight = _empShift.IsActiveNight;
                                    id.IsHoliday = _empShift.IsHoliday;
                                    ShiftInfoViewModel shift = new ShiftInfoViewModel()
                                    {
                                        Description = item.shiftInfo.Description,
                                        ClientCount = item.shiftInfo.ClientCount,
                                        EmployeeCount = item.shiftInfo.EmployeeCount,
                                        StartDate = hasDay.Date,
                                        StartTime = item.shiftInfo.StartTime,
                                        LocationType = item.shiftInfo.LocationType,
                                        EndDate = IsDateDiff ? hasDay.Date.AddDays(1) : hasDay.Date,
                                        EndTime = item.shiftInfo.EndTime,
                                        OtherLocation = _ShiftInfo.OtherLocation,
                                        IsPublished = false,
                                        LocationId = item.shiftInfo.LocationId,
                                        StartTimeString = hasDay.Date.Date.Add(item.shiftInfo.StartTime).ToString("hh:mm tt"),
                                        EndTimeString = IsDateDiff ? hasDay.Date.AddDays(1).Add(item.shiftInfo.EndTime).ToString("hh:mm tt") : hasDay.Date.Add(item.shiftInfo.EndTime).ToString("hh:mm tt"),
                                        Duration = item.shiftInfo.Duration,
                                        LocationName = item.shiftInfo.LocationId > 0 ? _dbContext.Location.Where(x => x.LocationId == item.shiftInfo.LocationId).Select(x => x.Name).FirstOrDefault() : item.shiftInfo.OtherLocation,
                                        Reminder = item.shiftInfo.Reminder,
                                        EmployeeId = id.EmployeeId,
                                        Name = id.Name,
                                        StatusId = _empShift.StatusId,
                                        StatusName = "Pending"
                                    };

                                    shift.Id = _ShiftInfo.Id;
                                    list.Add(shift);
                                }

                                foreach (var id in ClientShiftInfoViewModel)
                                {
                                    ClientShiftInfo _clientShift = new ClientShiftInfo();
                                    _clientShift.ClientId = id.ClientId;
                                    _clientShift.ShiftId = _ShiftInfo.Id;
                                    _clientShift.IsDeleted = false;
                                    _clientShift.CreatedDate = DateTime.Now;
                                    _clientShift.IsActive = true;
                                    _clientShift.CreatedById = await _ISessionService.GetUserId();
                                    await _dbContext.ClientShiftInfo.AddAsync(_clientShift);
                                    _dbContext.SaveChanges();
                                    id.Id = _clientShift.Id;
                                }
                                foreach (var id in ServiceTypeViewModel)
                                {
                                    ServiceTypeShiftInfo _ServiceTypeShiftInfo = new ServiceTypeShiftInfo();
                                    _ServiceTypeShiftInfo.ServiceTypeId = id.ServiceTypeId;
                                    _ServiceTypeShiftInfo.ShiftId = _ShiftInfo.Id;
                                    _ServiceTypeShiftInfo.IsDeleted = false;
                                    _ServiceTypeShiftInfo.CreatedDate = DateTime.Now;
                                    _ServiceTypeShiftInfo.IsActive = true;
                                    _ServiceTypeShiftInfo.CreatedById = await _ISessionService.GetUserId();
                                    await _dbContext.ServiceTypeShiftInfo.AddAsync(_ServiceTypeShiftInfo);
                                }


                                _dbContext.SaveChanges();

                                Message += "Shift Created for " + _ShiftInfo.Description + " Start Date :" + _ShiftInfo.StartDate.ToShortDateString() + " Time: " + _ShiftInfo.StartDate.Add(_ShiftInfo.StartTime).ToString(@"hh\:mm tt") + "\n";

                                _dbContext.Database.CommitTransaction();
                            }
                            else
                            {
                                // Message += "Shift Already Exist for " + shiftExist.Description + " Start Date :" + shiftExist.StartDate.ToShortDateString() + " Time: " + shiftExist.StartDate.Add(shiftExist.StartTime).ToString(@"hh\:mm tt") + "\n";

                            }
                        }

                    }

                    //.Distinct().;
                    if (list != null && list.Any())
                    {
                        response.Total = list.Count;
                        response.StatusCode = ResponseCode.Ok;
                        response.Message = Message;
                        response.Status = (int)Number.One;
                        response.ResponseData = new ShiftViewLoadTemplate() { ShiftInfoViewModel = list.ToList(), ShiftResponseList = shiftResponseList };

                    }
                    else
                    {
                        response = response.NotFound();
                    }
                }
                else
                {
                    response = response.NotFound();
                }

            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
                _dbContext.Database.RollbackTransaction();
            }
            return response;
        }

        public async Task<ApiResponse> SaveShiftTemplate(string templateName, List<int> shiftList)
        {
            ApiResponse response = new ApiResponse();
            try
            {

                var ExistTemplate = _dbContext.ShiftTemplate.FirstOrDefault(x => x.Name == templateName && x.IsActive == true && x.IsDeleted == false);

                if (ExistTemplate == null)
                {
                    _dbContext.Database.BeginTransaction();

                    ShiftTemplate _ShiftTemplate = new ShiftTemplate();
                    _ShiftTemplate.Name = templateName;
                    _ShiftTemplate.IsDeleted = false;
                    _ShiftTemplate.CreatedDate = DateTime.Now;
                    _ShiftTemplate.IsActive = true;
                    _ShiftTemplate.CreatedById = await _ISessionService.GetUserId();
                    await _dbContext.ShiftTemplate.AddAsync(_ShiftTemplate);
                    _dbContext.SaveChanges();

                    foreach (var request in shiftList.Distinct())
                    {

                        var ExistUser = _dbContext.ShiftInfo.FirstOrDefault(x => x.Id == request).Id;
                        if (ExistUser > 0)
                        {
                            ShiftInfoTemplate _ShiftInfo = new ShiftInfoTemplate();
                            _ShiftInfo.ShiftId = ExistUser;
                            _ShiftInfo.ShiftTemplateId = _ShiftTemplate.Id;
                            _ShiftInfo.IsDeleted = false;
                            _ShiftInfo.CreatedDate = DateTime.Now;
                            _ShiftInfo.IsActive = true;
                            _ShiftInfo.CreatedById = await _ISessionService.GetUserId();
                            await _dbContext.ShiftInfoTemplate.AddAsync(_ShiftInfo);
                            _dbContext.SaveChanges();

                        }
                        else
                        {
                            response.Failed("Shift not found or already completed");
                        }

                    }
                    response = response.Success(_ShiftTemplate);
                    _dbContext.Database.CommitTransaction();
                }
                else
                {
                    response.AlreadyExist();
                }

            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
                _dbContext.Database.RollbackTransaction();

            }
            return response;

        }

        public async Task<ApiResponse> UpdateShift(UpdateShiftInfoCommand request)
        {
            ApiResponse response = new ApiResponse();
            List<ShiftResponse> shiftResponseList = new List<ShiftResponse>();
            try
            {
                if (request.Id > 0 && !string.IsNullOrEmpty(request.Description) && request.EmployeeId != null && request.EmployeeId.Any() && request.ClientId != null && request.ClientId.Any() && request.StartDate != DateTime.MinValue)
                {

                    var startTime = TimeSpan.Parse(request.StartTime);
                    var endTime = TimeSpan.Parse(request.EndTime);


                    var ExistShift = CheckShiftExist(request.StartDate.Date, request.EndDate.Date, startTime, endTime, request.EmployeeId.Select(x => x.EmployeeId).ToArray(), shiftResponseList, request.Id);

                    // var ShiftOverlap = IsShiftOverLoaded(request.StartDate.Date, request.EndDate.Date, startTime, endTime, request.EmployeeId.Select(x => x.EmployeeId).ToArray(), shiftResponseList, request.Id);
                    var isOnLeave = IsEmployeeOnLeave(request.StartDate.Date, request.EndDate.Date, request.EmployeeId.Select(x => x.EmployeeId).ToList(), shiftResponseList);

                    var ExistUser = _dbContext.ShiftInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive);
                    if (ExistUser != null && !ExistShift && !isOnLeave)
                    {
                        await _dbContext.Database.BeginTransactionAsync();
                        ExistUser.Description = request.Description;
                        ExistUser.EmployeeCount = request.EmployeeId.Count();
                        ExistUser.ClientCount = request.ClientId.Count();
                        ExistUser.StartDate = request.StartDate.Date;
                        ExistUser.StartTime = TimeSpan.Parse(request.StartTime);
                        ExistUser.EndDate = request.EndDate.Date;
                        ExistUser.EndTime = TimeSpan.Parse(request.EndTime);
                        // Added by Rohit on 30 oct 2020
                        ExistUser.StartUtcDate = request.StartDate.Date.Add(TimeSpan.Parse(request.StartTime));
                        ExistUser.EndUtcDate = request.EndDate.Date.Add(TimeSpan.Parse(request.EndTime));
                        // End added
                        ExistUser.IsPublished = request.IsPublished;
                        ExistUser.LocationId = request.LocationId;
                        ExistUser.OtherLocation = request.OtherLocation;
                        ExistUser.Reminder = request.Reminder;
                        ExistUser.Duration = ExistUser.EndDate.Date.Add(ExistUser.EndTime).Subtract(ExistUser.StartDate.Date.Add(ExistUser.StartTime)).TotalHours;
                        ExistUser.IsDeleted = false;
                        ExistUser.LocationType = request.LocationType;
                        ExistUser.Remark = request.Remark;
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        _dbContext.ShiftInfo.Update(ExistUser);
                        var emplShiftInfo = _dbContext.EmployeeShiftInfo.Where(x => x.ShiftId == request.Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                        _dbContext.EmployeeShiftInfo.RemoveRange(_dbContext.EmployeeShiftInfo.Where(x => x.ShiftId == request.Id && x.IsDeleted == false && x.IsActive));
                        _dbContext.ClientShiftInfo.RemoveRange(_dbContext.ClientShiftInfo.Where(x => x.ShiftId == request.Id && x.IsDeleted == false && x.IsActive));
                        _dbContext.ServiceTypeShiftInfo.RemoveRange(_dbContext.ServiceTypeShiftInfo.Where(x => x.ShiftId == request.Id && x.IsDeleted == false && x.IsActive));
                        await _dbContext.SaveChangesAsync();
                        foreach (var id in request.EmployeeId)
                        {
                            EmployeeShiftInfo _empShift = new EmployeeShiftInfo();
                            _empShift.IsAccepted = (emplShiftInfo.EmployeeId == id.EmployeeId ? emplShiftInfo.IsAccepted : false);
                            _empShift.EmployeeId = id.EmployeeId;
                            _empShift.ShiftId = ExistUser.Id;
                            _empShift.IsSleepOver = id.IsSleepOver;
                            _empShift.IsDeleted = false;
                            _empShift.CreatedDate = DateTime.Now;
                            _empShift.IsActive = true;
                            _empShift.StatusId = request.StatusId;
                            _empShift.CreatedById = await _ISessionService.GetUserId();
                            await _dbContext.EmployeeShiftInfo.AddAsync(_empShift);
                        }

                        foreach (var id in request.ClientId)
                        {
                            ClientShiftInfo _clientShift = new ClientShiftInfo();
                            _clientShift.ClientId = id;
                            _clientShift.ShiftId = ExistUser.Id;
                            _clientShift.IsDeleted = false;
                            _clientShift.CreatedDate = DateTime.Now;
                            _clientShift.IsActive = true;
                            _clientShift.CreatedById = await _ISessionService.GetUserId();
                            await _dbContext.ClientShiftInfo.AddAsync(_clientShift);
                        }
                        foreach (var id in request.ServiceTypeId)
                        {
                            ServiceTypeShiftInfo _ServiceTypeShiftInfo = new ServiceTypeShiftInfo();
                            _ServiceTypeShiftInfo.ServiceTypeId = id;
                            _ServiceTypeShiftInfo.ShiftId = ExistUser.Id;
                            _ServiceTypeShiftInfo.IsDeleted = false;
                            _ServiceTypeShiftInfo.CreatedDate = DateTime.Now;
                            _ServiceTypeShiftInfo.IsActive = true;
                            _ServiceTypeShiftInfo.CreatedById = await _ISessionService.GetUserId();
                            await _dbContext.ServiceTypeShiftInfo.AddAsync(_ServiceTypeShiftInfo);
                        }

                        _dbContext.SaveChanges();
                        _dbContext.Database.CommitTransaction();
                        response = response.Success(ExistUser);
                    }
                    else
                    {
                        response.StatusCode = ResponseCode.Ok;
                        response.Message = "Shift is exist or shift over loaded";
                        response.Status = (int)Number.Zero;
                        response.ResponseData = shiftResponseList;
                    }
                }
                else
                {
                    response.ValidationError();
                }

            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                response.Failed(ex.Message);

            }
            return response;
        }

        public ApiResponse GetShiftPopOverInfo(GetShiftPopOverInfoQuery request)
        {
            ApiResponse response = new ApiResponse();
            ShiftPopOverInfoViewModel model = new ShiftPopOverInfoViewModel();
            try
            {

                model.EmployeeShiftInfoViewModel = (from shiftdata in _dbContext.ShiftInfo
                                                    join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                                    join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id

                                                    where shiftdata.IsDeleted == false && shiftdata.IsActive == true && shiftdata.Id == request.ShiftId
                                                    select new EmployeeShiftInfoViewModel
                                                    {
                                                        Id = emShift.Id,
                                                        EmployeeId = emInfo.Id,
                                                        IsSleepOver = emShift.IsSleepOver,
                                                        StatusId = emShift.StatusId.Value,
                                                        StatusName = _dbContext.StandardCode.Where(x => x.ID == emShift.StatusId.Value).Select(x => x.CodeDescription).FirstOrDefault(),
                                                        Name = emInfo.FirstName + " " + (emInfo.MiddleName == null ? "" : emInfo.MiddleName) + " " + emInfo.LastName,
                                                    }).ToList();


                model.ClientShiftInfoViewModel = (from shiftdata in _dbContext.ShiftInfo
                                                  join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                                  join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                                  where shiftdata.IsDeleted == false && shiftdata.IsActive == true && shiftdata.Id == request.ShiftId
                                                  select new ClientShiftInfoViewModel
                                                  {
                                                      //Id = shiftdata.Id,
                                                      ClientId = clInfo.Id,
                                                      Name = clInfo.FirstName + " " + (clInfo.MiddleName == null ? "" : clInfo.MiddleName) + " " + clInfo.LastName,
                                                  }).ToList();

                model.ServiceTypeViewModel = (from shiftdata in _dbContext.ShiftInfo
                                              join shiftservice in _dbContext.ServiceTypeShiftInfo on shiftdata.Id equals shiftservice.ShiftId
                                              join sc in _dbContext.ServiceDetails on shiftservice.ServiceTypeId equals sc.Id
                                              where shiftdata.IsDeleted == false && shiftdata.IsActive == true && shiftdata.Id == request.ShiftId
                                              select new ServiceTypeViewModel
                                              {
                                                  Id = shiftservice.Id,
                                                  ServiceTypeId = shiftservice.ServiceTypeId,
                                                  Name = sc.SupportItemName,
                                              }).ToList();
                response.SuccessWithOutMessage(model);
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }
        private bool IsActiveNight(DateTime date1, DateTime date2)
        {
            if (date1.Date == date2.Date || date1.Date > date2.Date)
            {
                return false;

            }
            else
            {
                if (date2.Date > date1.Date && date2 > date2.Date)
                {
                    return true;
                }

            }
            return false;
        }
        private bool IsHoliday(DateTime date1, DateTime date2)
        {
            var AvbempList = (from shiftdata in _dbContext.PublicHoliday
                              where shiftdata.IsDeleted == false && shiftdata.IsActive == true &&
                              (shiftdata.DateFrom.Value.Date >= date1.Date && shiftdata.DateFrom.Value.Date <= date2.Date ||
                              shiftdata.DateTo.Value.Date >= date1.Date && shiftdata.DateTo.Value.Date <= date2.Date ||
                              (shiftdata.DateFrom.Value.Date >= date1.Date && shiftdata.DateTo.Value.Date <= date2.Date)

                              )
                              select new
                              {
                                  shiftdata

                              }
                              ).Distinct().ToArray();
            if (AvbempList.Any())
            {
                return true;
            }

            return false;
        }

        private bool CheckShiftExist(DateTime StartDate, DateTime EndDate, TimeSpan StartTime, TimeSpan EndTime, int[] employeesId, List<ShiftResponse> shiftResponseList, int ShiftExclude = 0)
        {

            bool isExist = false;
            var shift = (from shiftdata in _dbContext.ShiftInfo
                         join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                         join eminfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals eminfo.Id
                         where shiftdata.IsDeleted == false && shiftdata.IsActive == true && eminfo.Status == true && eminfo.IsActive == true && eminfo.IsDeleted == false && ((shiftdata.StartDate.Date >= StartDate.Date && shiftdata.StartDate.Date <= EndDate.Date)
                                           || (shiftdata.EndDate.Date >= StartDate.Date && shiftdata.EndDate.Date <= EndDate.Date)) && (ShiftExclude == 0 || shiftdata.Id != ShiftExclude)// && ((shiftdata.StartDate.Date.Add(shiftdata.StartTime) >= StartDate.Date.Add(StartTime) && shiftdata.StartDate.Date.Add(shiftdata.StartTime) <= EndDate.Date.Add(EndTime)) || (shiftdata.EndDate.Date.Add(shiftdata.EndTime) <= StartDate.Date.Add(StartTime) && shiftdata.EndDate.Date.Add(shiftdata.EndTime) <= EndDate.Date.Add(EndTime)))
                         && employeesId.Contains(emShift.EmployeeId)
                         select new
                         {
                             ShiftInfo = shiftdata,
                             EmployeeFirstName = eminfo.FirstName,
                             EmployeeMiddelName = eminfo.MiddleName,
                             EmployeeLastName = eminfo.LastName,
                             //EmployeeId = emShift.EmployeeId
                         }).ToList();

            if (shift.Any())
            {
                foreach (var item in shift)
                {
                    var StartDateTime = item.ShiftInfo.StartDate.Add(item.ShiftInfo.StartTime);
                    var EndDateTime = item.ShiftInfo.EndDate.Add(item.ShiftInfo.EndTime);
                    var SelectedStartDateTime = StartDate.Add(StartTime);
                    var SelectedendDateTime = EndDate.Add(EndTime);
                    if ((SelectedStartDateTime > StartDateTime && SelectedStartDateTime < EndDateTime) || (SelectedendDateTime > StartDateTime && SelectedendDateTime < EndDateTime))
                    {
                        ShiftResponse shiftResponse = new ShiftResponse();
                        shiftResponse.Description = item.ShiftInfo.Description;
                        shiftResponse.Date = item.ShiftInfo.StartDate.ToShortDateString();
                        shiftResponse.Time = item.ShiftInfo.StartDate.Add(item.ShiftInfo.StartTime).ToString(@"hh\:mm tt");
                        shiftResponse.Action = "Shift Already Exist";
                        shiftResponse.EmployeeName = item.EmployeeFirstName + " " + (string.IsNullOrEmpty(item.EmployeeMiddelName) ? "" : item.EmployeeMiddelName + " ") + item.EmployeeLastName;
                        shiftResponseList.Add(shiftResponse);
                        isExist = true;
                    }
                }



            }


            return isExist;

        }
        private bool IsEmployeeOnLeave(DateTime StartDate, DateTime EndDate, List<int> employeesId, List<ShiftResponse> shiftResponseList)
        {

            bool isExist = false;
            var shift = (from leave in _dbContext.EmployeeLeaveInfo
                         join eminfo in _dbContext.EmployeePrimaryInfo on leave.EmployeeId equals eminfo.Id
                         where leave.IsDeleted == false && leave.IsActive == true && eminfo.Status == true && eminfo.IsActive == true
                         && eminfo.IsDeleted == false &&
                         ((leave.DateFrom.HasValue && leave.DateFrom.Value.Date >= StartDate.Date && leave.DateFrom.HasValue && leave.DateFrom.Value.Date <= EndDate.Date)
                                           || (leave.DateTo.HasValue && leave.DateTo.Value.Date >= StartDate.Date && leave.DateTo.HasValue && leave.DateTo.Value.Date <= EndDate.Date)) && employeesId.Contains(eminfo.Id)
                         select new
                         {
                             EmployeeFirstName = eminfo.FirstName,
                             EmployeeMiddelName = eminfo.MiddleName,
                             EmployeeLastName = eminfo.LastName,
                             //EmployeeId = emShift.EmployeeId
                         }).ToList();

            if (shift.Any())
            {
                foreach (var item in shift)
                {
                    ShiftResponse shiftResponse = new ShiftResponse();
                    shiftResponse.Description = "Employee is on leave";
                    shiftResponse.Date = StartDate.ToShortDateString();
                    //shiftResponse.Time = ;
                    shiftResponse.Action = "Employee is on leave";
                    shiftResponse.EmployeeName = item.EmployeeFirstName + " " + (string.IsNullOrEmpty(item.EmployeeMiddelName) ? "" : item.EmployeeMiddelName + " ") + item.EmployeeLastName;
                    shiftResponseList.Add(shiftResponse);
                    isExist = true;
                }

            }


            return isExist;

        }
        private bool IsShiftOverLoaded(DateTime StartDate, DateTime EndDate, TimeSpan StartTime, TimeSpan EndTime, int[] employeesId, List<ShiftResponse> shiftResponseList, int ShiftExclude = 0)
        {
            bool IsOverload = false;
            var ShifsAlreadyAdded = (from shiftdata in _dbContext.ShiftInfo
                                     join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                     join eminfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals eminfo.Id
                                     where shiftdata.IsDeleted == false && shiftdata.IsActive == true && eminfo.Status == true && eminfo.IsActive == true && eminfo.IsDeleted == false && ((shiftdata.StartDate.Date >= StartDate.Date && shiftdata.StartDate.Date <= EndDate.Date)
                                           || (shiftdata.EndDate.Date >= StartDate.Date && shiftdata.EndDate.Date <= EndDate.Date))
                                           && employeesId.Contains(emShift.EmployeeId) && (ShiftExclude == 0 || shiftdata.Id != ShiftExclude)
                                     select new
                                     {
                                         ShiftInfo = shiftdata,
                                         EmployeeFirstName = eminfo.FirstName,
                                         EmployeeMiddelName = eminfo.MiddleName,
                                         EmployeeLastName = eminfo.LastName,
                                     }).ToList();
            if (ShifsAlreadyAdded != null && ShifsAlreadyAdded.Any())
            {
                double TotalDurationForDay = 0;
                double currentHours = 0;
                foreach (var item in ShifsAlreadyAdded)
                {
                    var IsShiftExist = item.ShiftInfo;

                    if (IsShiftExist.EndDate.Date > IsShiftExist.StartDate.Date)
                    {
                        if (IsShiftExist.StartDate.Date == StartDate.Date && StartDate.Date == EndDate.Date)
                        {
                            var ForADayTime = new TimeSpan(0, 0, 0);
                            TotalDurationForDay += IsShiftExist.EndDate.Date.Add(ForADayTime).Subtract(IsShiftExist.StartDate.Date.Add(IsShiftExist.StartTime)).TotalHours;

                            currentHours = EndDate.Date.Add(EndTime).Subtract(StartDate.Date.Add(StartTime)).TotalHours;
                        }
                        else
                        {
                            if (EndDate.Date > IsShiftExist.EndDate.Date)
                            {
                                var ForADayTime = new TimeSpan(0, 0, 0);
                                TotalDurationForDay += IsShiftExist.EndDate.Date.Add(IsShiftExist.EndTime).Subtract(IsShiftExist.EndDate.Date.Add(ForADayTime)).TotalHours;

                                currentHours = EndDate.Date.Add(ForADayTime).Subtract(StartDate.Date.Add(StartTime)).TotalHours;
                            }
                            else
                            {
                                if (IsShiftExist.EndDate.Date == EndDate.Date && StartDate.Date == EndDate.Date && ShifsAlreadyAdded.Count > 1)
                                {
                                    var ForADayTime = new TimeSpan(0, 0, 0);
                                    TotalDurationForDay += IsShiftExist.EndDate.Date.Add(IsShiftExist.EndTime).Subtract(IsShiftExist.EndDate.Date.Add(ForADayTime)).TotalHours;
                                    currentHours = EndDate.Date.Add(EndTime).Subtract(StartDate.Date.Add(StartTime)).TotalHours;
                                }
                                else
                                {
                                    TotalDurationForDay += IsShiftExist.EndDate.Date.Add(IsShiftExist.EndTime).Subtract(IsShiftExist.StartDate.Date.Add(IsShiftExist.StartTime)).TotalHours;
                                    currentHours = EndDate.Date.Add(EndTime).Subtract(StartDate.Date.Add(StartTime)).TotalHours;
                                }
                            }
                        }
                    }
                    else
                    {
                        TotalDurationForDay += IsShiftExist.EndDate.Date.Add(IsShiftExist.EndTime).Subtract(IsShiftExist.StartDate.Date.Add(IsShiftExist.StartTime)).TotalHours;
                        currentHours = EndDate.Date.Add(EndTime).Subtract(StartDate.Date.Add(StartTime)).TotalHours;
                    }



                    if ((TotalDurationForDay + currentHours) > 8)
                    {
                        ShiftResponse shiftResponse = new ShiftResponse();
                        shiftResponse.Description = item.ShiftInfo.Description;
                        shiftResponse.Date = IsShiftExist.StartDate.ToShortDateString();
                        shiftResponse.Time = IsShiftExist.StartDate.Add(IsShiftExist.StartTime).ToString(@"hh\:mm tt");
                        shiftResponse.Action = "Shift Over Loaded";
                        shiftResponse.EmployeeName = item.EmployeeFirstName + " " + (string.IsNullOrEmpty(item.EmployeeMiddelName) ? "" : item.EmployeeMiddelName + " ") + item.EmployeeLastName;
                        shiftResponseList.Add(shiftResponse);
                        IsOverload = true;

                    }

                }
            }

            return IsOverload;

        }
    }
}

using LHSAPI.Application.Interface;
using LHSAPI.Application.Shift.Models;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Common.CommonMethods;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;
using INotification = LHSAPI.Application.Interface.INotification;

namespace LHSAPI.Application.Shift.Commands.Create.AddShiftInfo
{
    public class AddShiftInfoCommandHandler : IRequestHandler<AddShiftInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private readonly INotification _Notification;
        private readonly IMessageService _IMessageService;
        public AddShiftInfoCommandHandler(LHSDbContext context, ISessionService ISessionService, INotification notification, IMessageService IMessageService)
        {
            _context = context;
            _ISessionService = ISessionService;
            _Notification = notification;
            _IMessageService = IMessageService;



        }

        public async Task<ApiResponse> Handle(AddShiftInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            string Message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(request.Description) && request.EmployeeId != null && request.EmployeeId.Any() && request.ClientId != null && request.ClientId.Any() && request.StartDate != DateTime.MinValue)
                {
                    var dates = new List<DateTime>();
                    switch (request.ShiftRepeat)
                    {
                        case ShiftRepeatType.Tomorrow:
                            dates = CommonFunction.getDates(request.StartDate.Date, request.EndDate.Date);
                            response = await SaveShift(dates, request, new DayOfWeek[] { request.StartDate.Date.DayOfWeek, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday });
                            break;
                        case ShiftRepeatType.FullWeekly:
                            dates = CommonFunction.getDates(request.StartDate.Date, request.EndDate.Date, ShiftRepeatDays.Weekly);
                            response = await SaveShift(dates, request, new DayOfWeek[] { request.StartDate.Date.DayOfWeek });
                            break;
                        case ShiftRepeatType.FortNightly:
                            dates = CommonFunction.getDates(request.StartDate.Date, request.EndDate.Date, ShiftRepeatDays.FourthNightly);
                            response = await SaveShift(dates, request, new DayOfWeek[] { request.StartDate.Date.DayOfWeek });
                            break;
                        case ShiftRepeatType.SpecifiesDays:
                            dates = CommonFunction.getDates(request.StartDate.Date, request.EndDate.Date);
                            response = await SaveShift(dates, request, new DayOfWeek[] { request.StartDate.Date.DayOfWeek, DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday });

                            break;
                        case ShiftRepeatType.Month:
                            dates = CommonFunction.getDates(request.StartDate.Date, request.EndDate.Date, ShiftRepeatDays.Month);
                            response = await SaveShift(dates, request, new DayOfWeek[] { request.StartDate.Date.DayOfWeek });
                            break;
                        case ShiftRepeatType.WeekDays:
                            dates = CommonFunction.getDates(request.StartDate.Date, request.EndDate.Date);
                            response = await SaveShift(dates, request, new DayOfWeek[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday });
                            break;
                        case ShiftRepeatType.Sat:
                            dates = CommonFunction.getDates(request.StartDate.Date, request.EndDate.Date);
                            response = await SaveShift(dates, request, new DayOfWeek[] { DayOfWeek.Saturday });
                            break;
                        case ShiftRepeatType.Sun:
                            dates = CommonFunction.getDates(request.StartDate.Date, request.EndDate.Date);
                            response = await SaveShift(dates, request, new DayOfWeek[] { DayOfWeek.Sunday });
                            break;
                        default:
                            dates = CommonFunction.getDates(request.StartDate.Date, request.EndDate.Date, 1);
                            response = await SaveShift(dates, request, new DayOfWeek[] { request.StartDate.Date.DayOfWeek });
                            break;
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
        private async Task<ApiResponse> SaveShift(List<DateTime> DateList, AddShiftInfoCommand request, DayOfWeek[] dayOfWeeks)
        {
            ApiResponse response = new ApiResponse();
            try
            {

                string Message = string.Empty;
                List<ShiftResponse> shiftResponseList = new List<ShiftResponse>();
                var startHours = TimeSpan.Parse(request.StartTime).Hours;
                //if (startHours == 0)
                //{
                //  startHours = 24;
                //}
                bool IsDateDiff = TimeSpan.Parse(request.EndTime).Hours == 00 || TimeSpan.Parse(request.EndTime).Hours < startHours ? true : false;
                if (request.ShiftRepeat.ToLower() == "specifiesdays")
                {
                    DateList.RemoveAt(DateList.Count() - 1);
                }
                foreach (var ldate in DateList)
                {
                    if (dayOfWeeks.Contains(ldate.Date.DayOfWeek))
                    {
                        ShiftResponse shiftResponse = new ShiftResponse();
                        var startTime = TimeSpan.Parse(request.StartTime);
                        var endTime = TimeSpan.Parse(request.EndTime);
                        var EndDate = IsDateDiff ? ldate.AddDays(1) : ldate.Date;


                        var ExistShift = CheckShiftExist(ldate, EndDate, startTime, endTime, request.EmployeeId.Select(x => x.EmployeeId).ToArray(), shiftResponseList);
                        if (ExistShift)
                        {
                            continue;
                        }
                        //ExistShift = IsShiftOverLoaded(ldate, EndDate, startTime, endTime, request.EmployeeId.Select(x => x.EmployeeId).ToArray(), shiftResponseList);
                        //if (ExistShift)
                        //{
                        //  continue;
                        //}
                        ExistShift = IsEmployeeOnLeave(ldate, EndDate, request.EmployeeId.Select(x => x.EmployeeId).ToList(), shiftResponseList);
                        if (ExistShift)
                        {
                            continue;
                        }

                        //   var shiftIsExistDate = _context.ShiftInfo.FirstOrDefault(x => x.StartDate.Date == ldate.Date && x.EndDate.Date == EndDate && x.IsActive == true);

                        ShiftInfo _ShiftInfo = new ShiftInfo();
                        _ShiftInfo.Description = request.Description;
                        _ShiftInfo.EmployeeCount = request.EmployeeId.Count();
                        _ShiftInfo.ClientCount = request.ClientId.Count();
                        _ShiftInfo.StartDate = ldate;
                        _ShiftInfo.StartTime = startTime;
                        _ShiftInfo.EndDate = EndDate;
                        _ShiftInfo.EndTime = endTime;
                        //_ShiftInfo.StartUtcDate = TimeZoneInfo.ConvertTimeToUtc(ldate.Date.Add(startTime), TimeZoneInfo.FindSystemTimeZoneById("Indian Standard Time"));
                        //_ShiftInfo.EndUtcDate = TimeZoneInfo.ConvertTimeToUtc(EndDate.Date.Add(endTime), TimeZoneInfo.FindSystemTimeZoneById("Indian Standard Time"));
                        _ShiftInfo.StartUtcDate = (ldate.Date.Add(startTime)).ToUniversalTime();
                        _ShiftInfo.EndUtcDate = (EndDate.Date.Add(endTime)).ToUniversalTime();
                        _ShiftInfo.IsPublished = request.IsPublished;
                        _ShiftInfo.LocationId = request.LocationId;
                        _ShiftInfo.OtherLocation = request.OtherLocation;
                        _ShiftInfo.Reminder = request.Reminder;
                        _ShiftInfo.IsDeleted = false;
                        _ShiftInfo.CreatedDate = DateTime.Now;
                        _ShiftInfo.IsActive = true;
                        _ShiftInfo.CreatedById = await _ISessionService.GetUserId();
                        _ShiftInfo.LocationType = request.LocationType;
                        _ShiftInfo.Duration = _ShiftInfo.EndDate.Date.Add(_ShiftInfo.EndTime).Subtract(_ShiftInfo.StartDate.Date.Add(_ShiftInfo.StartTime)).TotalHours;
                        _ShiftInfo.ShiftRepeatType = request.ShiftRepeat;

                        await _context.Database.BeginTransactionAsync();

                        await _context.ShiftInfo.AddAsync(_ShiftInfo);
                        _context.SaveChanges();
                        foreach (var id in request.EmployeeId)
                        {
                            EmployeeShiftInfo _empShift = new EmployeeShiftInfo();
                            _empShift.EmployeeId = id.EmployeeId;
                            _empShift.IsSleepOver = id.IsSleepOver;
                            _empShift.ShiftId = _ShiftInfo.Id;
                            _empShift.IsDeleted = false;
                            _empShift.IsActiveNight = id.IsSleepOver ? false : IsActiveNight(_ShiftInfo.StartDate.Date.Add(_ShiftInfo.StartTime), _ShiftInfo.EndDate.Date.Add(_ShiftInfo.EndTime));
                            //_empShift.IsActiveNight = id.IsSleepOver ? false : true;
                            _empShift.CreatedDate = DateTime.Now;
                            _empShift.IsHoliday = id.IsSleepOver ? false : IsHoliday(_ShiftInfo.StartDate.Date.Add(_ShiftInfo.StartTime), _ShiftInfo.EndDate.Date.Add(_ShiftInfo.EndTime));
                            _empShift.IsActive = true;
                            _empShift.StatusId = request.StatusId;
                            _empShift.CreatedById = await _ISessionService.GetUserId();
                            await _context.EmployeeShiftInfo.AddAsync(_empShift);
                            var employee = _context.EmployeePrimaryInfo.Where(x => x.Id == _empShift.EmployeeId).FirstOrDefault();
                            string EmpEmailId = employee.EmailId;
                            string UserName = employee.FirstName;
                            if (!string.IsNullOrEmpty(EmpEmailId))
                            {
                                string emailBody = _IMessageService.GetEmailTemplate();
                                Message += "Shift Created for " + _ShiftInfo.Description + " Start Date :" + _ShiftInfo.StartDate.ToShortDateString() + " Time: " + _ShiftInfo.StartDate.Add(_ShiftInfo.StartTime).ToString(@"hh\:mm tt") + "\n";
                                string Subject = "Shift Created";
                                emailBody = emailBody.Replace("{Message}", Message);
                                emailBody = emailBody.Replace("{Subject}", Subject);
                                emailBody = emailBody.Replace("{UserName}", UserName);
                                _IMessageService.SendingEmails(EmpEmailId, Subject, emailBody);
                            }

                            await _Notification.SaveNotification(new LHSAPI.Domain.Entities.Notification
                            {
                                Email = true,
                                EventName = "Shift Created",
                                EmployeeId = id.EmployeeId,
                                Description = _ShiftInfo.Description + " Shift Created",
                                IsEmailSent = true
                            },
                Services.NotiFicationSaveMode.Employee);

                            // Message += "Shift Created for " + _ShiftInfo.Description + " Start Date :" + _ShiftInfo.StartDate.ToShortDateString() + " Time: " + _ShiftInfo.StartDate.Add(_ShiftInfo.StartTime).ToString(@"hh\:mm tt") + "\n";
                            shiftResponse.Description = _ShiftInfo.Description;
                            shiftResponse.Date = _ShiftInfo.StartDate.ToShortDateString();
                            shiftResponse.Time = _ShiftInfo.StartDate.Add(_ShiftInfo.StartTime).ToString(@"hh\:mm tt");
                            shiftResponse.Action = "Shift Created";
                            shiftResponse.EmployeeName = employee.FirstName + " " + (string.IsNullOrEmpty(employee.MiddleName) ? "" : employee.MiddleName + " ") + employee.LastName;
                            shiftResponseList.Add(shiftResponse);
                        }

                        foreach (var id in request.ClientId)
                        {
                            ClientShiftInfo _clientShift = new ClientShiftInfo();
                            _clientShift.ClientId = id;
                            _clientShift.ShiftId = _ShiftInfo.Id;
                            _clientShift.IsDeleted = false;
                            _clientShift.CreatedDate = DateTime.Now;
                            _clientShift.IsActive = true;
                            _clientShift.CreatedById = await _ISessionService.GetUserId();
                            await _context.ClientShiftInfo.AddAsync(_clientShift);
                        }

                        _context.SaveChanges();
                        if (request.ServiceTypeId != null)
                        {
                            foreach (var id in request.ServiceTypeId)
                            {
                                ServiceTypeShiftInfo _ServiceTypeShiftInfo = new ServiceTypeShiftInfo();
                                _ServiceTypeShiftInfo.ServiceTypeId = id;
                                _ServiceTypeShiftInfo.ShiftId = _ShiftInfo.Id;
                                _ServiceTypeShiftInfo.IsDeleted = false;
                                _ServiceTypeShiftInfo.CreatedDate = DateTime.Now;
                                _ServiceTypeShiftInfo.IsActive = true;
                                _ServiceTypeShiftInfo.CreatedById = await _ISessionService.GetUserId();
                                await _context.ServiceTypeShiftInfo.AddAsync(_ServiceTypeShiftInfo);
                            }

                            _context.SaveChanges();
                        }

                        _context.Database.CommitTransaction();




                    }
                }

                response.StatusCode = ResponseCode.Ok;
                response.Message = null;
                response.Status = (int)Number.One;
                response.ResponseData = shiftResponseList;
                return response;
            }
            catch (Exception ex)
            {
                _context.Database.RollbackTransaction();
                response.StatusCode = ResponseCode.ServerError;
                response.Message = ex.Message;
                response.Status = (int)Number.Zero;
                response.ResponseData = null;
            }
            return response;
        }

        private bool CheckShiftExist(DateTime StartDate, DateTime EndDate, TimeSpan StartTime, TimeSpan EndTime, int[] employeesId, List<ShiftResponse> shiftResponseList)
        {
            bool isExist = false;
            var shift = (from shiftdata in _context.ShiftInfo
                         join emShift in _context.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                         join eminfo in _context.EmployeePrimaryInfo on emShift.EmployeeId equals eminfo.Id
                         where shiftdata.IsDeleted == false && shiftdata.IsActive == true && eminfo.Status == true && eminfo.IsActive == true && eminfo.IsDeleted == false && ((shiftdata.StartDate.Date >= StartDate.Date && shiftdata.StartDate.Date <= EndDate.Date)
                                           || (shiftdata.EndDate.Date >= StartDate.Date && shiftdata.EndDate.Date <= EndDate.Date))// && ((shiftdata.StartDate.Date.Add(shiftdata.StartTime) >= StartDate.Date.Add(StartTime) && shiftdata.StartDate.Date.Add(shiftdata.StartTime) <= EndDate.Date.Add(EndTime)) || (shiftdata.EndDate.Date.Add(shiftdata.EndTime) <= StartDate.Date.Add(StartTime) && shiftdata.EndDate.Date.Add(shiftdata.EndTime) <= EndDate.Date.Add(EndTime)))
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
                    if ((SelectedStartDateTime >= StartDateTime && SelectedStartDateTime <= EndDateTime) || (SelectedendDateTime >= StartDateTime && SelectedendDateTime <= EndDateTime)
                        )
                    {
                        ShiftResponse shiftResponse = new ShiftResponse();
                        shiftResponse.Description = item.ShiftInfo.Description;
                        shiftResponse.Date = item.ShiftInfo.StartDate.ToShortDateString();
                        shiftResponse.Time = item.ShiftInfo.StartDate.Add(item.ShiftInfo.StartTime).ToString(@"hh\:mm tt");
                        shiftResponse.Action = "Employee already booked for another shift";
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
            var shift = (from leave in _context.EmployeeLeaveInfo
                         join eminfo in _context.EmployeePrimaryInfo on leave.EmployeeId equals eminfo.Id
                         where leave.IsDeleted == false && leave.IsActive == true && eminfo.Status == true && eminfo.IsActive == true
                         && eminfo.IsDeleted == false &&
                         //((leave.DateFrom.HasValue && leave.DateFrom.Value.Date >= StartDate.Date && leave.DateTo.HasValue && StartDate.Date <= leave.DateTo.Value.Date)
                         //                  || (leave.DateFrom.HasValue && leave.DateFrom.Value.Date <= EndDate.Date && leave.DateTo.HasValue && leave.DateTo.Value.Date >= EndDate.Date)) 
                         (StartDate >= leave.DateFrom.Value.Date && EndDate <= leave.DateFrom.Value.Date)
                                           && employeesId.Contains(eminfo.Id)
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
        private bool IsShiftOverLoaded(DateTime StartDate, DateTime EndDate, TimeSpan StartTime, TimeSpan EndTime, int[] employeesId, List<ShiftResponse> shiftResponseList)
        {
            bool IsOverload = false;
            var ShifsAlreadyAdded = (from shiftdata in _context.ShiftInfo
                                     join emShift in _context.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                     join eminfo in _context.EmployeePrimaryInfo on emShift.EmployeeId equals eminfo.Id
                                     where shiftdata.IsDeleted == false && shiftdata.IsActive == true && eminfo.Status == true && eminfo.IsActive == true && eminfo.IsDeleted == false && ((shiftdata.StartDate.Date >= StartDate.Date && shiftdata.StartDate.Date <= EndDate.Date)
                                           || (shiftdata.EndDate.Date >= StartDate.Date && shiftdata.EndDate.Date <= EndDate.Date))
                                           && employeesId.Contains(emShift.EmployeeId)
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
            var AvbempList = (from shiftdata in _context.PublicHoliday
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




    }

    public static class ShiftRepeatType
    {
        public const string Tomorrow = "Tomorrow";
        public const string SpecifiesDays = "SpecifiesDays";
        public const string FullWeekly = "FullWeekly";
        public const string FortNightly = "FortNightly";
        public const string Month = "Month";
        public const string None = "None";
        public const string WeekDays = "WeekDays";
        public const string Sat = "Sat";
        public const string Sun = "Sun";

    }
    public static class ShiftRepeatDays
    {
        public const int Tomorrow = 1;
        public const int Weekly = 7;
        public const int FourthNightly = 14;
        public const int Month = 30;

    }

}

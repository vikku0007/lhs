using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using INotification = LHSAPI.Application.Interface.INotification;

namespace LHSAPI.Application.Shift.Commands.Create.AddAcceptShift
{
    public class AddAcceptShiftInfoCommandHandler : IRequestHandler<AddAcceptShiftInfoCommand, ApiResponse>, IRequestHandler<UpdateCheckoutShiftCommand, ApiResponse>
    {

        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private const int ActiveNightHours = 6;
        private readonly INotification _Notification;
        private readonly IMessageService _IMessageService;
        private readonly IConfiguration _configuration;
        public AddAcceptShiftInfoCommandHandler(LHSDbContext context, ISessionService ISessionService, INotification Notification, IMessageService IMessageService, IConfiguration configuration)
        {
            _context = context;
            _ISessionService = ISessionService;
            _Notification = Notification;
            _IMessageService = IMessageService;
            _configuration = configuration;
        }
        public async Task<ApiResponse> Handle(AddAcceptShiftInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0 && request.ShiftId > 0 && request.CheckInDate != DateTime.MinValue && !string.IsNullOrEmpty(request.CheckInTime))
                {
                    var checkinTime = TimeSpan.Parse(request.CheckInTime);
                    var employeeShift = _context.EmployeeShiftTracker.FirstOrDefault(x => x.ShiftId == request.ShiftId && x.IsActive == true && x.IsDeleted == false);
                    // x.CheckInDate == request.CheckInDate && x.CheckInTime == checkinTime && );
                    if (employeeShift == null)
                    {
                        EmployeeShiftTracker _shift = new EmployeeShiftTracker();
                        _shift.ShiftId = request.ShiftId;
                        _shift.EmployeeId = request.EmployeeId;
                        _shift.ClientId = request.ClientId;
                        _shift.CheckInDate = request.CheckInDate;
                        _shift.CheckInTime = checkinTime;
                        _shift.CheckInRemarks = request.CheckInRemarks;
                        _shift.IsApproveByAccounts = false;
                        _shift.IsLogin = true;
                        //_shift.ToDoList_Flag = request.ToDoList_Flag;
                        //_shift.AccidentIncident_Flag = request.AccidentIncident_Flag;
                        //_shift.ProgressNotes_Flag = request.ProgressNotes_Flag;
                        _shift.CreatedDate = DateTime.Now;
                        _shift.IsActive = true;
                        _shift.IsDeleted = false;
                        await _context.EmployeeShiftTracker.AddAsync(_shift);
                        _context.SaveChanges();
                        ShiftHistoryInfo _shifthistory = new ShiftHistoryInfo();
                        _shifthistory.ShiftId = request.ShiftId;
                        _shifthistory.EmployeeId = request.EmployeeId;
                        _shifthistory.CheckInDate = request.CheckInDate;
                        _shifthistory.CheckInTime = checkinTime;
                        _shifthistory.CreatedById = await _ISessionService.GetUserId();
                        _shifthistory.CreatedDate = DateTime.Now;
                        _shifthistory.IsActive = true;
                        _shifthistory.IsDeleted = false;
                        await _context.ShiftHistoryInfo.AddAsync(_shifthistory);
                        _context.SaveChanges();
                        await _Notification.SaveNotification(new LHSAPI.Domain.Entities.Notification
                        {
                            Email = true,
                            EventName = "Employee Accepted Shift ",
                            EmployeeId = request.EmployeeId,
                            Description = "Employee Accepted Shift",
                            IsAdminAlert = true,
                            IsEmailSent = true
                        }, Services.NotiFicationSaveMode.Employee);
                        response = response.Success(_shift);
                        string EmailId = _context.EmployeePrimaryInfo.Where(x => x.Id == request.EmployeeId && x.IsActive == true && x.IsDeleted == false).Select(x => x.EmailId).FirstOrDefault();
                        string UserName = _context.EmployeePrimaryInfo.Where(x => x.Id == request.EmployeeId && x.IsActive == true && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault();

                        if (!string.IsNullOrEmpty(EmailId))
                        {
                            string emailBody = _IMessageService.GetShiftTemplate();
                            string Message = "You are successfully logged in.";
                            string Date = "Date :" + _shift.CheckInDate.Value.ToShortDateString();
                            string Time = "Time: " + _shift.CheckInDate.Value.Add(_shift.CheckInTime.Value).ToString(@"hh\:mm tt");
                            string Subject = "Successfully Logged in";
                            emailBody = emailBody.Replace("{Message}", Message);
                            emailBody = emailBody.Replace("{Subject}", Subject);
                            emailBody = emailBody.Replace("{UserName}", UserName);
                            emailBody = emailBody.Replace("{Date}", Date);
                            emailBody = emailBody.Replace("{Time}", Time);
                            _IMessageService.SendingEmailsAsynWithCC(EmailId, Subject, emailBody, _configuration.GetSection("SMTP:CC_RegistrationEmail").Value, null);
                        }
                    }
                    else
                    {
                        response.AlreadyExist();
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
        public async Task<ApiResponse> Handle(UpdateCheckoutShiftCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var employeeShift = _context.EmployeeShiftTracker.FirstOrDefault(x => x.ShiftId == request.ShiftId &&
                x.EmployeeId == request.EmployeeId && x.IsActive == true && x.IsDeleted == false && x.IsShiftCompleted == false);
                var shiftInfo = (from shiftdata in _context.ShiftInfo
                                 join emShift in _context.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                 where shiftdata.IsDeleted == false && shiftdata.IsActive == true && shiftdata.Id == request.ShiftId && emShift.EmployeeId == request.EmployeeId
                                 select new
                                 {
                                     shiftdata,
                                     emShift
                                 }).FirstOrDefault();
                if (employeeShift != null && shiftInfo != null)
                {
                    employeeShift.CheckOutDate = request.CheckOutDate.Date;
                    employeeShift.CheckOutTime = TimeSpan.Parse(request.CheckOutTime);
                    employeeShift.CheckOutRemarks = request.CheckOutRemarks;
                    // Added on 28 Jan 2021
                    employeeShift.IsCheckoutByWeb = request.IsCheckoutByWeb;
                    employeeShift.IsCheckoutByApp = request.IsCheckoutByApp;
                    //var duration = employeeShift.CheckOutDate.Value.Date.Add(employeeShift.CheckOutTime.Value).Subtract(employeeShift.CheckInDate.Date.Add(employeeShift.CheckInTime));
                    //employeeShift.TotalDuration = duration.Minutes > 10 ? duration.TotalHours : Math.Round(duration.TotalHours);
                    var duration = request.CheckOutDate.Date.Add(TimeSpan.Parse(request.CheckOutTime)).Subtract(employeeShift.CheckInDate.Value.Date.Add(employeeShift.CheckInTime.Value));
                    employeeShift.TotalDuration = duration.TotalHours;
                    var activenight = shiftInfo.emShift.IsActiveNight ? CalculateActiveNightHours(employeeShift.CheckInDate.Value.Date.Add(employeeShift.CheckInTime.Value),
                        request.CheckOutDate.Date.Add(TimeSpan.Parse(request.CheckOutTime))) : 0;
                    employeeShift.ActiveNightDuration = activenight;
                    //var sleepOverNight = shiftInfo.emShift.IsSleepOver ? CalculateSleepOverNightHours(employeeShift.CheckInDate.Add(employeeShift.CheckInTime), employeeShift.CheckOutDate.Value.Date.Add(employeeShift.CheckOutTime.Value)) : 0;
                    var sleepOverNight = shiftInfo.emShift.IsSleepOver ? CalculateSleepOverNightHours(employeeShift.CheckInDate.Value.Date.Add(employeeShift.CheckInTime.Value),
                        request.CheckOutDate.Date.Add(TimeSpan.Parse(request.CheckOutTime))) : 0;
                    employeeShift.SleepOverDuration = sleepOverNight;
                    employeeShift.DayDuration = employeeShift.TotalDuration - (activenight + sleepOverNight);
                    employeeShift.UpdatedDate = DateTime.Now;
                    employeeShift.UpdateById = await _ISessionService.GetUserId();
                    employeeShift.IsShiftCompleted = true;
                    employeeShift.IsApproveByAccounts = false;
                    employeeShift.IsLogin = false;
                    employeeShift.ToDoList_Flag = request.ToDoList_Flag;
                    employeeShift.AccidentIncident_Flag = request.AccidentIncident_Flag;
                    employeeShift.ProgressNotes_Flag = request.ProgressNotes_Flag;
                    employeeShift.IsShiftOnTime = IsShiftOnTime(employeeShift.CheckInDate.Value, employeeShift.CheckOutDate.Value.Date.Add(employeeShift.CheckOutTime.Value), shiftInfo.shiftdata.StartDate.Date.Add(shiftInfo.shiftdata.StartTime), shiftInfo.shiftdata.EndDate.Date.Add(shiftInfo.shiftdata.EndTime));
                    shiftInfo.emShift.StatusId = _context.StandardCode.Where(x => x.CodeData == "ShiftStatus" && x.CodeDescription == "Complete").FirstOrDefault().ID;
                    _context.EmployeeShiftInfo.Update(shiftInfo.emShift);
                    _context.EmployeeShiftTracker.Update(employeeShift);

                    _context.SaveChanges();
                    var ExistUser = _context.ShiftHistoryInfo.FirstOrDefault(x => x.ShiftId == request.ShiftId && x.IsActive == true && x.IsDeleted == false);
                    if (ExistUser != null)
                    {
                        ExistUser.ShiftId = request.ShiftId;
                        ExistUser.EmployeeId = request.EmployeeId;
                        ExistUser.CheckOutDate = request.CheckOutDate;
                        ExistUser.CheckOutTime = TimeSpan.Parse(request.CheckOutTime);
                        _context.ShiftHistoryInfo.Update(ExistUser);
                        _context.SaveChanges();
                    }


                    await _Notification.SaveNotification(new LHSAPI.Domain.Entities.Notification
                    {
                        Email = true,
                        EventName = "Employee Log Out",
                        EmployeeId = request.EmployeeId,
                        Description = shiftInfo.shiftdata.Description + " Employee Log Out",
                        IsAdminAlert = false,
                        IsEmailSent = true
                    }, Services.NotiFicationSaveMode.Employee);
                    response = response.Success(employeeShift);
                    string EmailId = _context.EmployeePrimaryInfo.Where(x => x.Id == request.EmployeeId && x.IsActive == true && x.IsDeleted == false).Select(x => x.EmailId).FirstOrDefault();
                    string UserName = _context.EmployeePrimaryInfo.Where(x => x.Id == request.EmployeeId && x.IsActive == true && x.IsDeleted == false).Select(x => x.FirstName).FirstOrDefault();

                    if (!string.IsNullOrEmpty(EmailId))
                    {
                        string emailBody = _IMessageService.GetShiftTemplate();
                        string Message = "You are successfully log Out.";
                        string Date = "Date :" + employeeShift.CheckOutDate.Value.ToShortDateString();
                        string Time = " Time: " + employeeShift.CheckOutDate.Value.Date.Add(employeeShift.CheckOutTime.Value).ToString(@"hh\:mm tt");
                        string Subject = "Successfully Log Out";
                        emailBody = emailBody.Replace("{Message}", Message);
                        emailBody = emailBody.Replace("{Subject}", Subject);
                        emailBody = emailBody.Replace("{UserName}", UserName);
                        emailBody = emailBody.Replace("{Date}", Date);
                        emailBody = emailBody.Replace("{Time}", Time);
                        _IMessageService.SendingEmailsAsynWithCC(EmailId, Subject, emailBody, _configuration.GetSection("SMTP:CC_RegistrationEmail").Value, null);
                    }
                }
                else if (employeeShift == null)
                {
                    response.Failed("You haven't login to the shift yet!");
                }
                else
                {
                    response.Failed("Shift not found or completed");
                }
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }
        private bool IsShiftOnTime(DateTime CheckinDatetime, DateTime CheckoutDatetime, DateTime StartDatetime, DateTime EndDatetime)
        {
            bool lshiftOnTime = false;
            var checkin = CheckinDatetime.Subtract(StartDatetime).TotalMinutes;
            var checkout = CheckoutDatetime.Subtract(EndDatetime).TotalMinutes;
            if ((checkin <= 15 || checkin <= -15) && (checkout <= -15 || checkout <= 15))
            {
                lshiftOnTime = true;
            }
            else
            {
                lshiftOnTime = false;
            }



            return lshiftOnTime;
        }
        private double CalculateActiveNightHours(DateTime CheckInDateTime, DateTime CheckOutDateTime)
        {
            double returnValue = 0;

            TimeSpan activeNightDuration = TimeSpan.Zero;

            if (CheckOutDateTime > CheckInDateTime)
            {
                DateTime dt = CheckInDateTime.Date.AddHours(24);
                DateTime dt1 = CheckOutDateTime.Date.AddHours(6);
                if (CheckInDateTime < dt && CheckOutDateTime > dt1)
                {
                    TimeSpan timeSpan = new TimeSpan(6, 0, 0);
                    activeNightDuration = timeSpan;
                }
                else if (CheckInDateTime < dt && CheckOutDateTime < dt1)
                {
                    var time = CheckOutDateTime - dt;
                    activeNightDuration = time;

                }
                else if (CheckInDateTime > dt && CheckOutDateTime < dt1)
                {
                    var time = CheckOutDateTime - CheckInDateTime;
                    activeNightDuration = time;
                }
                else if (CheckInDateTime > dt && CheckOutDateTime > dt1)
                {
                    //var time = dt1.TimeOfDay - CheckInDateTime.TimeOfDay;
                    var time = dt1 - CheckInDateTime;
                    activeNightDuration = time;
                }
                returnValue = activeNightDuration.TotalHours;
            }
            return returnValue;

        }

        private double CalculateSleepOverNightHours(DateTime CheckInDateTime, DateTime CheckOutDateTime)
        {
            double returnValue = 0;

            //if (CheckOutDateTime > CheckInDateTime)
            //{
            //    var sleepOverNightHours = CheckOutDateTime.TimeOfDay.Subtract(CheckInDateTime.TimeOfDay);
            //    var checkoutTime = CheckOutDateTime.TimeOfDay;
            //    if (checkoutTime.TotalHours >= 6)
            //    {
            //        var midnightDate = CheckOutDateTime.Date;
            //        var sleepOverDate = midnightDate.AddHours(6);
            //        var timeduration = sleepOverDate.TimeOfDay.Subtract(CheckInDateTime.TimeOfDay);
            //        returnValue = timeduration.TotalHours;
            //    }
            //    else
            //    {
            //        returnValue = sleepOverNightHours.TotalHours;
            //    }
            //}
            TimeSpan sleepoverDuration = TimeSpan.Zero;

            if (CheckOutDateTime > CheckInDateTime)
            {
                DateTime dt = CheckInDateTime.Date.AddHours(22);
                DateTime dt1 = CheckOutDateTime.Date.AddHours(6);
                if (CheckInDateTime < dt && CheckOutDateTime > dt1)
                {
                    TimeSpan timeSpan = new TimeSpan(8, 0, 0);
                    sleepoverDuration = timeSpan;
                }
                else if (CheckInDateTime < dt && CheckOutDateTime < dt1)
                {
                    var time = CheckOutDateTime - dt;
                    sleepoverDuration = time;

                }
                else if (CheckInDateTime > dt && CheckOutDateTime < dt1)
                {
                    var time = CheckOutDateTime - CheckInDateTime;
                    sleepoverDuration = time;
                }
                else if (CheckInDateTime > dt && CheckOutDateTime > dt1)
                {
                    var time = dt1 - CheckInDateTime;
                    //var time = dt1.TimeOfDay - CheckInDateTime.TimeOfDay;
                    sleepoverDuration = time;
                }
                returnValue = sleepoverDuration.TotalHours;
            }
            return returnValue;
        }
    }
}

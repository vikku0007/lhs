using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using LHSAPI.Persistence.DbContext;
using LHSAPI.Application.Interface;
using LHSAPI.Domain.Entities;

namespace LHSAPI.Application.PayRoll.Commands.Create.AddShiftCheckoutDetails
{
    public class AddShiftCheckoutHandler : IRequestHandler<AddShiftCheckoutCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddShiftCheckoutHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }
        public async Task<ApiResponse> Handle(AddShiftCheckoutCommand request, CancellationToken cancellationToken)
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
                    employeeShift.CheckInDate = request.CheckInDate.Date;
                    employeeShift.CheckInTime = TimeSpan.Parse(request.CheckInTime.ToString());
                    employeeShift.CheckOutDate = request.CheckOutDate.Date;
                    employeeShift.CheckOutTime = TimeSpan.Parse(request.CheckOutTime.ToString());
                    employeeShift.AdminCheckOutRemark = request.Remark;
                    var duration = request.CheckOutDate.Date.Add(TimeSpan.Parse(request.CheckOutTime.ToString())).Subtract(request.CheckInDate.Date.Add(TimeSpan.Parse( request.CheckInTime.ToString())));
                    employeeShift.TotalDuration = duration.TotalHours;
                    var activenight = shiftInfo.emShift.IsActiveNight ? CalculateActiveNightHours(employeeShift.CheckInDate.Value.Date.Add(employeeShift.CheckInTime.Value),
                        request.CheckOutDate.Date.Add(TimeSpan.Parse(request.CheckOutTime.ToString()))) : 0;
                    employeeShift.ActiveNightDuration = activenight;
                    var sleepOverNight = shiftInfo.emShift.IsSleepOver ? CalculateSleepOverNightHours(employeeShift.CheckInDate.Value.Date.Add(employeeShift.CheckInTime.Value),
                        request.CheckOutDate.Date.Add(TimeSpan.Parse(request.CheckOutTime.ToString()))) : 0;
                    employeeShift.SleepOverDuration = sleepOverNight;
                    employeeShift.DayDuration = employeeShift.TotalDuration - (activenight + sleepOverNight);
                    employeeShift.UpdatedDate = DateTime.Now;
                    employeeShift.UpdateById = await _ISessionService.GetUserId();
                    employeeShift.IsShiftCompleted = true;
                    employeeShift.IsApproveByAccounts = false;
                    employeeShift.IsLogin = false;
                    employeeShift.ToDoList_Flag = false;
                    employeeShift.AccidentIncident_Flag = false;
                    employeeShift.ProgressNotes_Flag = false;
                    employeeShift.IsActive = true;
                    employeeShift.IsDeleted = false;
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
                        ExistUser.CheckOutTime = TimeSpan.Parse(request.CheckOutTime.ToString());
                        ExistUser.CheckInDate = request.CheckInDate;
                        ExistUser.CheckInTime = TimeSpan.Parse(request.CheckInTime.ToString());
                        _context.ShiftHistoryInfo.Update(ExistUser);
                        _context.SaveChanges();
                    }
                    else
                    {
                        ShiftHistoryInfo historyInfo = new ShiftHistoryInfo();
                        historyInfo.ShiftId = request.ShiftId;
                        historyInfo.EmployeeId = request.EmployeeId;
                        historyInfo.CheckInDate = request.CheckInDate;
                        historyInfo.CheckInTime = TimeSpan.Parse(request.CheckInTime);
                        historyInfo.CheckOutDate = request.CheckOutDate;
                        historyInfo.CreatedDate = DateTime.Now;
                        historyInfo.IsActive = true;
                        historyInfo.IsDeleted = false;
                        historyInfo.CheckOutTime = TimeSpan.Parse(request.CheckOutTime);
                        _context.ShiftHistoryInfo.Add(historyInfo);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    EmployeeShiftTracker tracker = new EmployeeShiftTracker();
                    tracker.CheckOutDate = request.CheckOutDate.Date;
                    tracker.CheckOutTime = TimeSpan.Parse(request.CheckOutTime.ToString());
                    tracker.CheckOutRemarks = request.Remark;
                    tracker.CheckInDate = request.CheckInDate.Date;
                    tracker.CheckInTime = TimeSpan.Parse(request.CheckInTime);
                    var duration = request.CheckOutDate.Date.Add(TimeSpan.Parse(request.CheckOutTime.ToString())).Subtract(request.CheckInDate.Date.Add(TimeSpan.Parse(request.CheckInTime)));
                    tracker.TotalDuration = duration.TotalHours;
                    var activenight = shiftInfo.emShift.IsActiveNight ? CalculateActiveNightHours(request.CheckInDate.Date.Add(TimeSpan.Parse(request.CheckInTime)),
                        request.CheckOutDate.Date.Add(TimeSpan.Parse(request.CheckOutTime.ToString()))) : 0;
                    tracker.ActiveNightDuration = activenight;
                    var sleepOverNight = shiftInfo.emShift.IsSleepOver ? CalculateSleepOverNightHours(request.CheckInDate.Date.Add(TimeSpan.Parse(request.CheckInTime)),
                        request.CheckOutDate.Date.Add(TimeSpan.Parse(request.CheckOutTime.ToString()))) : 0;
                    tracker.SleepOverDuration = sleepOverNight;
                    tracker.DayDuration = tracker.TotalDuration - (activenight + sleepOverNight);
                    tracker.CreatedDate = DateTime.Now;
                    tracker.UpdatedDate = DateTime.Now;
                    tracker.UpdateById = await _ISessionService.GetUserId();
                    tracker.IsShiftCompleted = true;
                    tracker.IsApproveByAccounts = false;
                    tracker.IsLogin = false;
                    tracker.ToDoList_Flag = false;
                    tracker.AccidentIncident_Flag = false;
                    tracker.ProgressNotes_Flag = false;
                    tracker.EmployeeId = request.EmployeeId;
                    tracker.ShiftId = request.ShiftId;
                    tracker.IsDeleted = false;
                    tracker.IsActive = true;
                    tracker.IsShiftOnTime = IsShiftOnTime(request.CheckInDate, request.CheckOutDate.Date.Add(TimeSpan.Parse(request.CheckOutTime)), shiftInfo.shiftdata.StartDate.Date.Add(shiftInfo.shiftdata.StartTime), shiftInfo.shiftdata.EndDate.Date.Add(shiftInfo.shiftdata.EndTime));
                    shiftInfo.emShift.StatusId = _context.StandardCode.Where(x => x.CodeData == "ShiftStatus" && x.CodeDescription == "Complete").FirstOrDefault().ID;
                    _context.EmployeeShiftInfo.Update(shiftInfo.emShift);
                    _context.EmployeeShiftTracker.Add(tracker);
                    _context.SaveChanges();
                    var ExistUser = _context.ShiftHistoryInfo.FirstOrDefault(x => x.ShiftId == request.ShiftId && x.IsActive == true && x.IsDeleted == false);
                    if (ExistUser != null)
                    {
                        ExistUser.ShiftId = request.ShiftId;
                        ExistUser.EmployeeId = request.EmployeeId;
                        ExistUser.CheckOutDate = request.CheckOutDate;
                        ExistUser.CheckOutTime = TimeSpan.Parse(request.CheckOutTime.ToString());
                        ExistUser.CheckInDate = request.CheckInDate;
                        ExistUser.CheckInTime = TimeSpan.Parse(request.CheckInTime.ToString());
                        _context.ShiftHistoryInfo.Update(ExistUser);
                        _context.SaveChanges();
                    }
                    else
                    {
                        ShiftHistoryInfo historyInfo = new ShiftHistoryInfo();
                        historyInfo.ShiftId = request.ShiftId;
                        historyInfo.EmployeeId = request.EmployeeId;
                        historyInfo.CheckInDate = request.CheckInDate;
                        historyInfo.CheckInTime = TimeSpan.Parse(request.CheckInTime);
                        historyInfo.CheckOutDate = request.CheckOutDate;
                        historyInfo.CreatedDate = DateTime.Now;
                        historyInfo.CheckOutTime = TimeSpan.Parse(request.CheckOutTime);
                        historyInfo.IsActive = true;
                        historyInfo.IsDeleted = false;
                        _context.ShiftHistoryInfo.Add(historyInfo);
                        _context.SaveChanges();
                    }
                }
                response.Success();
            }
            catch (Exception ex)
            {

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
                    sleepoverDuration = time;
                }
                returnValue = sleepoverDuration.TotalHours;
            }
            return returnValue;
        }
    }
}

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
using LHSAPI.Application.EmployeeStaff.Models;

namespace LHSAPI.Application.Shift.Queries.GetShiftHistoryDetails
{
    public class GetShiftHistoryDetailsHandler : IRequestHandler<GetShiftHistoryDetailsQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetShiftHistoryDetailsHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
        #region Get Shift History

        public async Task<ApiResponse> Handle(GetShiftHistoryDetailsQuery request, CancellationToken cancellationToken)
        {
            ShiftHistoryDetails _historyDetails = new ShiftHistoryDetails();
            ApiResponse response = new ApiResponse();
            try
            {
                var ExistUser = _dbContext.ShiftHistoryInfo.FirstOrDefault(x => x.ShiftId == request.ShiftId);
                if (ExistUser != null)
                {
                    _historyDetails.ShiftHistory = (from emp in _dbContext.ShiftHistoryInfo
                                                    join shiftTracker in _dbContext.EmployeeShiftTracker
            on emp.ShiftId equals shiftTracker.ShiftId
                                                    join empshiftInfo in _dbContext.EmployeeShiftInfo
                                                    on emp.ShiftId equals empshiftInfo.ShiftId

                                                    where emp.IsDeleted == false && emp.IsActive == true && emp.ShiftId == request.ShiftId
                                                    && empshiftInfo.IsActive == true && empshiftInfo.IsDeleted == false
                                                    select new LHSAPI.Application.EmployeeStaff.Models.ShiftHistory
                                                    {
                                                        ShiftId = emp.ShiftId,
                                                        EmployeeId = emp.EmployeeId,
                                                        CheckInTime = emp.CheckInTime,
                                                        CheckInDate = emp.CheckInDate,
                                                        CheckOutDate = emp.CheckOutDate,
                                                        CheckOutTime = emp.CheckOutTime,
                                                        CheckInTimestring = emp.CheckInDate == null ? null : emp.CheckInDate.Value.Date.Add(emp.CheckInTime.Value).ToString(@"hh\:mm tt"),
                                                        CheckOutTimestring = emp.CheckOutDate == null ? null : emp.CheckOutDate.Value.Date.Add(emp.CheckOutTime.Value).ToString(@"hh\:mm tt"),
                                                        Duration = Math.Round(emp.CheckOutDate.Value.Date.Add(emp.CheckOutTime.Value).Subtract(emp.CheckInDate.Value.Date.Add(emp.CheckInTime.Value)).TotalHours, 2),
                                                        IsShiftCompleted = shiftTracker.IsShiftCompleted,
                                                        IsActiveNight = empshiftInfo.IsActiveNight,
                                                        IsSleepover = empshiftInfo.IsSleepOver,
                                                    }).FirstOrDefault();
                    _historyDetails.ShiftHistory.CustomDuration = CalculateCustomDuration(_historyDetails.ShiftHistory);
                    _historyDetails.ProgressNotesList = (from item in _dbContext.ProgressNotesList
                                                         where item.IsActive == true && item.IsDeleted == false && item.ShiftId == request.ShiftId
                                                         select new LHSAPI.Application.EmployeeStaff.Models.ProgressNotesList
                                                         {
                                                             Id = item.Id,
                                                             ClientProgressNoteId = (item.ClientProgressNoteId),
                                                             ClientId = item.ClientId,
                                                             Date = item.Date,
                                                             Note9AMTo11AM = item.Note9AMTo11AM,
                                                             Note11AMTo1PM = item.Note11AMTo1PM,
                                                             Note1PMTo15PM = item.Note1PMTo15PM,
                                                             Note15PMTo17PM = item.Note15PMTo17PM,
                                                             Note17PMTo19PM = item.Note17PMTo19PM,
                                                             Note19PMTo21PM = item.Note19PMTo21PM,
                                                             Note21PMTo23PM = item.Note21PMTo23PM,
                                                             Note23PMTo1AM = item.Note23PMTo1AM,
                                                             Note1AMTo3AM = item.Note1AMTo3AM,
                                                             Note3AMTo5AM = item.Note3AMTo5AM,
                                                             Note5AMTo7AM = item.Note5AMTo7AM,
                                                             Note7AMTo9AM = item.Note7AMTo9AM,
                                                             CreatedDate = item.CreatedDate,
                                                             Summary = item.Summary,
                                                             OtherInfo = item.OtherInfo,

                                                         }).ToList();
                    _historyDetails.ToDoShift = (from item in _dbContext.ToDoShift
                                                 where item.IsActive == true && item.IsDeleted == false && item.ShiftId == request.ShiftId
                                                 select new LHSAPI.Application.EmployeeStaff.Models.ToDoShift
                                                 {
                                                     Id = item.Id,
                                                     ShiftId = item.ShiftId,
                                                     DateTime = item.DateTime,
                                                     ShiftType = item.ShiftType,
                                                     ShiftTypeName = _dbContext.StandardCode.Where(x => x.ID == item.ShiftType).Select(x => x.CodeDescription).FirstOrDefault()
                                                 }).FirstOrDefault();
                    _historyDetails.ShiftToDoList = (from emp in _dbContext.ShiftToDoList
                                                     where emp.IsActive == true && emp.IsDeleted == false && emp.ShiftId == request.ShiftId
                                                     select new LHSAPI.Application.EmployeeStaff.Models.ShiftToDoList
                                                     {
                                                         Id = emp.Id,
                                                         EmployeeId = emp.EmployeeId,
                                                         Description = emp.Description,
                                                         IsInitials = emp.IsInitials,
                                                         Initials = emp.Initials,
                                                         TodoItemId = emp.TodoItemId
                                                     }).ToList();
                    if (_historyDetails.ShiftHistory == null) _historyDetails.ShiftHistory = new LHSAPI.Application.EmployeeStaff.Models.ShiftHistory();
                    if (_historyDetails.ToDoShift == null) _historyDetails.ToDoShift = new LHSAPI.Application.EmployeeStaff.Models.ToDoShift();
                    if (_historyDetails.ShiftToDoList == null) _historyDetails.ShiftToDoList = new List<LHSAPI.Application.EmployeeStaff.Models.ShiftToDoList>();
                    if (_historyDetails.ProgressNotesList == null) _historyDetails.ProgressNotesList = new List<LHSAPI.Application.EmployeeStaff.Models.ProgressNotesList>();
                    response.SuccessWithOutMessage(_historyDetails);
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
        #endregion

        private string CalculateCustomDuration(ShiftHistory shiftdetail)
        {
            string temp = null;
            try
            {
                DateTime startDate = new DateTime();
                DateTime endDate = new DateTime();
                shiftdetail.CheckInDate = shiftdetail.CheckInDate.Value.Date.Add(shiftdetail.CheckInTime.Value);
                shiftdetail.CheckOutDate = shiftdetail.CheckOutDate.Value.Date.Add(shiftdetail.CheckOutTime.Value);
                if (shiftdetail.IsActiveNight == true)
                {
                    startDate = shiftdetail.CheckInDate.Value.Date.AddHours(24);
                    endDate = shiftdetail.CheckOutDate.Value.Date.AddHours(6);
                    double normalHours = 0;
                    double activeHours = 0;                    
                    if (shiftdetail.CheckInDate <= startDate && shiftdetail.CheckOutDate >= endDate)
                    {
                        normalHours = shiftdetail.Duration - 6;
                        activeHours = 6;
                    }
                    else if (shiftdetail.CheckInDate >= startDate && shiftdetail.CheckOutDate <= endDate)
                    {
                        normalHours = 0;
                        activeHours = shiftdetail.Duration;
                    }
                    else if (shiftdetail.CheckInDate >= startDate && shiftdetail.CheckOutDate >= endDate)
                    {
                        activeHours = (endDate - shiftdetail.CheckInDate.Value).TotalHours;
                        normalHours = shiftdetail.Duration - activeHours;
                    }
                    else if (shiftdetail.CheckInDate <= startDate && shiftdetail.CheckOutDate <= endDate)
                    {
                        activeHours = (shiftdetail.CheckOutDate.Value - startDate).TotalHours;
                        normalHours = shiftdetail.Duration - activeHours;
                    }

                    temp = "Normal Hours : " + Math.Round(normalHours,2) + " Hrs and Active Hours : " + activeHours + " Hrs";
                }
                else if (shiftdetail.IsSleepover == true)
                {
                    startDate = shiftdetail.CheckInDate.Value.Date.AddHours(22);
                    endDate = shiftdetail.CheckOutDate.Value.Date.AddHours(6);
                    double normalHours = 0;
                    double sleepHours = 0;                    
                    if (shiftdetail.CheckInDate <= startDate && shiftdetail.CheckOutDate >= endDate)
                    {
                        normalHours = shiftdetail.Duration - 8;
                        sleepHours = 1;
                    }
                    else if (shiftdetail.CheckInDate >= startDate && shiftdetail.CheckOutDate <= endDate)
                    {
                        normalHours = 0;
                        sleepHours = 1;
                    }
                    else if (shiftdetail.CheckInDate >= startDate && shiftdetail.CheckOutDate >= endDate)
                    {
                        sleepHours = (endDate - shiftdetail.CheckInDate.Value).TotalHours;
                        normalHours = shiftdetail.Duration - sleepHours;
                        sleepHours = 1;
                    }
                    else if (shiftdetail.CheckInDate <= startDate && shiftdetail.CheckOutDate <= endDate)
                    {
                        sleepHours = (shiftdetail.CheckOutDate.Value - startDate).TotalHours;
                        normalHours = shiftdetail.Duration - sleepHours;
                        sleepHours = 1;
                    }

                    temp = "Normal Hours : " + Math.Round(normalHours,2) + " Hrs and Sleep Over : " + sleepHours;
                }
                else
                {
                    double normalHours = shiftdetail.Duration;
                    temp = "Normal Hours : " + Math.Round(normalHours,2) + " Hrs";
                }
            }
            catch (Exception ex)
            {
            }
            return temp;
        }
    }
}

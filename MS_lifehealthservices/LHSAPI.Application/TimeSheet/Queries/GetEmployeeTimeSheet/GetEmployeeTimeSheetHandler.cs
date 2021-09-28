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
using LHSAPI.Application.Interface;

namespace LHSAPI.Application.TimeSheet.Queries.GetEmployeeTimeSheet
{
    public class GetEmployeeTimeSheetHandler : IRequestHandler<GetEmployeeTimeSheet, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        private readonly IShiftService _ShiftService;
        //   readonly ILoggerManager _logger;
        public GetEmployeeTimeSheetHandler(LHSDbContext dbContext, IShiftService ShiftService)
        {
            _dbContext = dbContext;
            // _logger = logger;
            _ShiftService = ShiftService;
        }
        #region Get Shift List

        public async Task<ApiResponse> Handle(GetEmployeeTimeSheet request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                var list = (from shiftdata in _dbContext.ShiftInfo
                            join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                            join emp in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emp.Id
                            join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                            join emShiftTracker in _dbContext.EmployeeShiftTracker on shiftdata.Id equals emShiftTracker.ShiftId
                            where shiftdata.IsDeleted == false && shiftdata.IsActive == true && emp.IsDeleted == false && emp.IsActive == true &&
                            emShiftTracker.IsShiftCompleted == true && (request.EmployeeId == 0 || emp.Id == request.EmployeeId)
                            && emShiftTracker.CheckInDate.Value.Date >= request.StartDate.Date && emShiftTracker.CheckOutDate.HasValue && emShiftTracker.CheckOutDate.Value.Date <= request.EndDate.Date
                            select new TimeSheetViewModel()
                            {
                                ShiftId = shiftdata.Id,
                                Description = shiftdata.Description,
                                StartDate = emShiftTracker.CheckInDate.Value.Date.Add(emShiftTracker.CheckInTime.Value),
                                StartTimeString = emShiftTracker.CheckInDate.Value.Date.Add(emShiftTracker.CheckInTime.Value).ToString(@"hh\:mm tt"),
                                EndTimeString = emShiftTracker.CheckOutDate.Value.Date.Add(emShiftTracker.CheckOutTime.Value).ToString(@"hh\:mm tt"),
                                Duration = Math.Round(emShiftTracker.TotalDuration, 2),
                                EndDate = emShiftTracker.CheckOutDate.Value.Date.Add(emShiftTracker.CheckOutTime.Value),
                                StatusId = emShift.StatusId,
                                StatusName = _dbContext.StandardCode.Where(x => x.ID == emShift.StatusId).Select(x => x.CodeDescription).FirstOrDefault(),
                                EmployeeId = emShift.EmployeeId,
                                IsActiveNight = emShift.IsActiveNight,
                                IsSleepOver = emShift.IsSleepOver
                            }
                  ).ToList();
                if (list.Count > 0)
                {
                    list = CalculateCustomDuration(list);
                }
                response.SuccessWithOutMessage(list);

            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }

        private List<TimeSheetViewModel> CalculateCustomDuration(List<TimeSheetViewModel> shiftdetails)
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

                        temp = "Normal Hours : " + Math.Round(normalHours,2) + " Hrs and Active Hours : " + activeHours + " Hrs";
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
        #endregion
    }
    public class TimeSheetViewModel
    {
        public int ShiftId { get; set; }

        public string Description { get; set; }

        public int? StatusId { get; set; }
        public string StatusName { get; set; }

        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Duration { get; set; }
        public string StartTimeString { get; set; }
        public string EndTimeString { get; set; }
        public string CustomDuration { get; set; }
        public bool IsActiveNight { get; set; }
        public bool IsSleepOver { get; set; }


    }
}

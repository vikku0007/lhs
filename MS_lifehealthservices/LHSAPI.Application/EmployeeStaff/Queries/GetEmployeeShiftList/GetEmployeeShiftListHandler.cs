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

namespace LHSAPI.Application.Shift.Queries.GetEmployeeShiftList
{
    public class GetEmployeeShiftListHandler : IRequestHandler<GetEmployeeShiftListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeShiftListHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
        #region Get Shift List

        public async Task<ApiResponse> Handle(GetEmployeeShiftListQuery request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                // List<ShiftInfoViewModel> list = new List<ShiftInfoViewModel>();
                var list = (from shiftdata in _dbContext.ShiftInfo
                                //join location in _dbContext.Location on shiftdata.LocationId equals location.LocationId
                            join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                            join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                            join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                            where emShift.IsDeleted == false && emShift.IsActive == true && emShift.EmployeeId == request.EmployeeId
                            && shiftdata.IsActive == true && shiftdata.IsDeleted == false
                            && (request.SearchTextBylocation == 0)
                            //location.LocationId == request.SearchTextBylocation)
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
                                LocationName = shiftdata.LocationId != null ? (_dbContext.Location.Where(x => x.LocationId == shiftdata.LocationId).Select(x => x.Name).FirstOrDefault()) : shiftdata.OtherLocation,
                                Reminder = shiftdata.Reminder,
                                CreatedDate = shiftdata.CreatedDate,
                                StatusName = _dbContext.StandardCode.Where(x => x.ID == emShift.StatusId).Select(x => x.CodeDescription).FirstOrDefault(),
                                IsLogin = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id && x.EmployeeId == request.EmployeeId).Select(x => x.IsLogin).FirstOrDefault(),
                                IsShiftCompleted = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id && x.EmployeeId == request.EmployeeId).Select(x => x.IsShiftCompleted).FirstOrDefault(),
                                IsActiveNight = emShift.IsActiveNight,
                                IsSleepOver = emShift.IsSleepOver,
                                ClientShiftInfoViewModel = (from data in _dbContext.ClientPrimaryInfo
                                                            join clShift in _dbContext.ClientShiftInfo on data.Id equals clShift.ClientId
                                                            where data.IsDeleted == false && data.IsActive == true && ((request.SearchByClientName == 0 && shiftdata.Id == shiftdata.Id) || (request.SearchByClientName > 0 && data.Id == request.SearchByClientName && shiftdata.Id == shiftdata.Id))
                                                             && clShift.ShiftId == shiftdata.Id
                                                            select new ClientShiftInfoViewModel
                                                            {
                                                                Id = shiftdata.Id,
                                                                ClientId = data.Id,
                                                                Name = data.FirstName + " " + (data.MiddleName == null ? "" : data.MiddleName) + " " + data.LastName,
                                                                DateOfBirth = data.DateOfBirth
                                                            }).ToList()
                            }).ToList();
                if (list != null && list.Any())
                {
                    list = CalculateCustomDuration(list);
                    switch (request.OrderBy)
                    {
                        case Common.Enums.Employee.EmployeeShiftOrderBy.description:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                list = list.OrderBy(x => x.Description).ToList();
                            }
                            else
                            {
                                list = list.OrderByDescending(x => x.Description).ToList();
                            }
                            break;

                        case Common.Enums.Employee.EmployeeShiftOrderBy.location:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                list = list.OrderBy(x => x.LocationName).ToList();
                            }
                            else
                            {
                                list = list.OrderByDescending(x => x.LocationName).ToList();
                            }
                            break;
                        case Common.Enums.Employee.EmployeeShiftOrderBy.status:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                list = list.OrderBy(x => x.StatusName).ToList();
                            }
                            else
                            {
                                list = list.OrderByDescending(x => x.StatusName).ToList();
                            }
                            break;
                        default:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                list = list.OrderBy(x => x.CreatedDate).ToList();
                            }
                            else
                            {
                                list = list.OrderByDescending(x => x.CreatedDate).ToList();
                            }

                            break;
                    }

                    var clientlist = list.OrderByDescending(x => x.Id).ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                    var totalCount = list.Count();
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(clientlist);



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

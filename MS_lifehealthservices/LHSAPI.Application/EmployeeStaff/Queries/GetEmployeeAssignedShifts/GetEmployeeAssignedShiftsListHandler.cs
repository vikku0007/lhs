using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using LHSAPI.Application.EmployeeStaff.Models;

namespace LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeAssignedShifts
{
    public class GetEmployeeAssignedShiftsListHandler : IRequestHandler<GetEmployeeAssignedShiftsQuery, ApiResponse>, IRequestHandler<GetEmployeeCalendarShiftsQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        public GetEmployeeAssignedShiftsListHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse> Handle(GetEmployeeAssignedShiftsQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var assignedShifts = (from shiftdata in _dbContext.ShiftInfo
                                      join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                      join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                                      //  join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                      join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                                      //  join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                      join emShiftT in _dbContext.EmployeeShiftTracker on shiftdata.Id equals emShiftT.ShiftId into gj
                                      from subpet in gj.DefaultIfEmpty()
                                      join location in _dbContext.Location on shiftdata.LocationId equals location.LocationId into gj1
                                      from subpet1 in gj1.DefaultIfEmpty()
                                      where shiftdata.IsDeleted == false && shiftdata.IsActive == true && emShift.EmployeeId == request.Id
                                      && shiftdata.StartDate.Date >= DateTime.Now.Date && subpet == null
                                      select new AssignedShiftInfoViewModel
                                      {
                                          Id = shiftdata.Id,
                                          EmployeeId = emShift.EmployeeId,
                                          Description = shiftdata.Description,
                                          // ClientName = clInfo.FirstName + " " + clInfo.LastName,
                                          Location = subpet1 == null ? shiftdata.OtherLocation : subpet1.Name,
                                          StartDate = shiftdata.StartDate.Date,
                                          EndDate = shiftdata.EndDate.Date,
                                          StartTime = shiftdata.StartTime,
                                          EndTime = shiftdata.EndTime,
                                          StartTimeString = shiftdata.StartDate.Date.Add(shiftdata.StartTime).ToString(@"hh\:mm tt"),
                                          EndTimeString = shiftdata.EndDate.Date.Add(shiftdata.EndTime).ToString(@"hh\:mm tt"),
                                          IsAccepted = emShift.IsAccepted,
                                          IsRejected = emShift.IsRejected,
                                          IsShiftCompleted = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id && x.EmployeeId == request.Id).Select(x => x.IsShiftCompleted).FirstOrDefault(),
                                          IsLogin = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id && x.EmployeeId == request.Id).Select(x => x.IsLogin).FirstOrDefault(),
                                          // ClientImgURL = _dbContext.ClientPicInfo.Where(x => x.ClientId == clInfo.Id).Select(x => x.Path).FirstOrDefault()
                                      }).Distinct().ToList();

                assignedShifts = assignedShifts.Where(x => x.IsShiftCompleted == false && x.IsAccepted == false).OrderBy(x => x.StartDate).ToList();
                var totalCount = assignedShifts.Count();
                assignedShifts = assignedShifts.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                foreach (var item in assignedShifts)
                {
                    item.ClientInfo = (from shiftdata in _dbContext.ShiftInfo
                                       join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                       join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                       where shiftdata.IsDeleted == false && shiftdata.IsActive == true && shiftdata.Id == item.Id
                                       select new ClientInfo
                                       {
                                           ShiftId = item.Id,
                                           ClientId = clInfo.Id,
                                           ClientName = clInfo.FirstName + " " + (clInfo.MiddleName == null ? "" : clInfo.MiddleName) + " " + clInfo.LastName,
                                           ClientImgURL = _dbContext.ClientPicInfo.Where(x => x.ClientId == clInfo.Id).Select(x => x.Path).FirstOrDefault()
                                       }).OrderByDescending(x => x.ClientName).ToList();
                }
                response.Total = totalCount;
                response.SuccessWithOutMessage(assignedShifts);
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public async Task<ApiResponse> Handle(GetEmployeeCalendarShiftsQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var assignedShifts = (from shiftdata in _dbContext.ShiftInfo
                                          //join location in _dbContext.Location on shiftdata.LocationId equals location.LocationId
                                      join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                      join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                                      //   join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                      join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                                      //  join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                      where shiftdata.IsDeleted == false && shiftdata.IsActive == true && emShift.EmployeeId == request.Id
                                      && (shiftdata.StartDate.Date >= request.FromDate && shiftdata.EndDate.Date <= request.ToDate)
                                      select new AssignedShiftInfoViewModel
                                      {
                                          Id = shiftdata.Id,
                                          Description = shiftdata.Description,
                                          //  ClientName = clInfo.FirstName + " " + clInfo.LastName,
                                          Location = shiftdata.LocationId != null ? (_dbContext.Location.Where(x => x.LocationId == shiftdata.LocationId && x.IsActive == true && x.IsDeleted == false).Select(x => x.Name).FirstOrDefault()) : shiftdata.OtherLocation,
                                          StartDate = shiftdata.StartDate.Date,
                                          EndDate = shiftdata.EndDate.Date,
                                          StartTime = shiftdata.StartTime,
                                          EndTime = shiftdata.EndTime,
                                          StartTimeString = shiftdata.StartDate.Date.Add(shiftdata.StartTime).ToString(@"hh\:mm tt"),
                                          EndTimeString = shiftdata.EndDate.Date.Add(shiftdata.EndTime).ToString(@"hh\:mm tt"),
                                          IsAccepted = emShift.IsAccepted,
                                          IsRejected = emShift.IsRejected,
                                          IsShiftCompleted = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id && x.EmployeeId == request.Id).Select(x => x.IsShiftCompleted).FirstOrDefault(),
                                          StatusName = status.CodeDescription
                                          // IsLogin = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id && x.EmployeeId == request.Id).Select(x => x.IsLogin).FirstOrDefault()
                                      }).Distinct().ToList();
                // assignedShifts = assignedShifts.Where(x => x.IsShiftCompleted == false).OrderBy(x => x.StartDate).ToList();
                var totalCount = assignedShifts.Count();
                response.Total = totalCount;
                response.SuccessWithOutMessage(assignedShifts);
            }
            catch (Exception ex)
            {

            }
            return response;
        }
    }
}

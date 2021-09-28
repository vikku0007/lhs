using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using LHSAPI.Application.Client.Models;

namespace LHSAPI.Application.Client.Queries.GetClientCurrentShifts
{
    public class GetClientCurrentShiftsQueryHandler : IRequestHandler<GetClientCurrentShiftsQuery, ApiResponse>, IRequestHandler<GetClientAssignedShiftsQuery, ApiResponse>,
        IRequestHandler<UpdateClientShiftCancelQuery, ApiResponse>, IRequestHandler<GetClientCalendarShifts, ApiResponse>
    {

        private readonly LHSDbContext _dbContext;
        public GetClientCurrentShiftsQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse> Handle(GetClientCurrentShiftsQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var assignedShifts = (from shiftdata in _dbContext.ShiftInfo
                                      join location in _dbContext.Location on shiftdata.LocationId equals location.LocationId
                                      join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                      join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                                      join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                      join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                                      join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                      where shiftdata.IsDeleted == false && shiftdata.IsActive == true &&
                                      clShift.ClientId == request.Id
                                      && shiftdata.StartDate.Date == DateTime.Now.Date
                                      select new ShiftInfoViewModel
                                      {
                                          Id = shiftdata.Id,
                                          Description = shiftdata.Description,
                                          ClientName = clInfo.FirstName + " " + clInfo.LastName,
                                          Location = location.Name,
                                          StartDate = shiftdata.StartDate.Date,
                                          EndDate = shiftdata.EndDate.Date,
                                          StartTime = shiftdata.StartTime,
                                          EndTime = shiftdata.EndTime,
                                          StartTimeString = shiftdata.StartDate.Date.Add(shiftdata.StartTime).ToString(@"hh\:mm tt"),
                                          EndTimeString = shiftdata.EndDate.Date.Add(shiftdata.EndTime).ToString(@"hh\:mm tt"),
                                          IsAccepted = emShift.IsAccepted,
                                          IsRejected = emShift.IsRejected,
                                          IsShiftCompleted = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id && x.EmployeeId == request.Id).Select(x => x.IsShiftCompleted).FirstOrDefault(),
                                          EmployeeId = request.Id,
                                          EmployeeName = emInfo.FirstName + " " + emInfo.LastName,
                                          StatusName = status.CodeDescription

                                      }).OrderBy(x => x.StartDate).Distinct().ToList();
                assignedShifts = assignedShifts.Where(x => x.IsShiftCompleted == false).ToList();
                var totalCount = assignedShifts.Count();
                response.Total = totalCount;
                response.SuccessWithOutMessage(assignedShifts);
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public async Task<ApiResponse> Handle(GetClientAssignedShiftsQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var assignedShifts = (from shiftdata in _dbContext.ShiftInfo
                                      join location in _dbContext.Location on shiftdata.LocationId equals location.LocationId
                                      join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                      join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                                      join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                      join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                                      join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                      where shiftdata.IsDeleted == false && shiftdata.IsActive == true && clShift.ClientId == request.Id
                                      && shiftdata.StartDate.Date >= DateTime.Now.Date && clShift.IsCancelled == false
                                      select new ShiftInfoViewModel
                                      {
                                          Id = shiftdata.Id,
                                          Description = shiftdata.Description,
                                          ClientName = clInfo.FirstName + " " + clInfo.LastName,
                                          Location = location.Name,
                                          StartDate = shiftdata.StartDate.Date,
                                          EndDate = shiftdata.EndDate.Date,
                                          StartTime = shiftdata.StartTime,
                                          EndTime = shiftdata.EndTime,
                                          StartTimeString = shiftdata.StartDate.Date.Add(shiftdata.StartTime).ToString(@"hh\:mm tt"),
                                          EndTimeString = shiftdata.EndDate.Date.Add(shiftdata.EndTime).ToString(@"hh\:mm tt"),
                                          IsAccepted = emShift.IsAccepted,
                                          IsRejected = emShift.IsRejected,
                                          IsShiftCompleted = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id && x.EmployeeId == request.Id).Select(x => x.IsShiftCompleted).FirstOrDefault(),
                                          EmployeeName = emInfo.FirstName + " " + emInfo.LastName,
                                          StatusName = status.CodeDescription
                                      }).Distinct().ToList();
                assignedShifts = assignedShifts.Where(x => x.IsShiftCompleted == false).OrderBy(x => x.StartDate).ToList();
                var totalCount = assignedShifts.Count();
                assignedShifts = assignedShifts.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                response.Total = totalCount;
                response.SuccessWithOutMessage(assignedShifts);
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public async Task<ApiResponse> Handle(UpdateClientShiftCancelQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request != null)
                {

                    var ExistEmp = _dbContext.ClientShiftInfo.FirstOrDefault(x => x.ShiftId == request.Id && x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false);
                    if (ExistEmp != null)
                    {
                        ExistEmp.IsCancelled = true;
                        ExistEmp.Remark = request.Remark;
                        _dbContext.ClientShiftInfo.Update(ExistEmp);
                        _dbContext.SaveChanges();
                        response.Update(ExistEmp);
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

        public async Task<ApiResponse> Handle(GetClientCalendarShifts request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var assignedShifts = (from shiftdata in _dbContext.ShiftInfo
                                      join location in _dbContext.Location on shiftdata.LocationId equals location.LocationId
                                      join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                      join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                                      join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                      join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                                      join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                      where shiftdata.IsDeleted == false && shiftdata.IsActive == true && clShift.ClientId == request.Id
                                      && (shiftdata.StartDate.Date >= request.FromDate.Date && shiftdata.EndDate.Date <= request.ToDate.Date)
                                      && clShift.IsCancelled == false
                                      select new ShiftInfoViewModel
                                      {
                                          Id = shiftdata.Id,
                                          Description = shiftdata.Description,
                                          ClientName = clInfo.FirstName + " " + clInfo.LastName,
                                          Location = location.Name,
                                          StartDate = shiftdata.StartDate.Date,
                                          EndDate = shiftdata.EndDate.Date,
                                          StartTime = shiftdata.StartTime,
                                          EndTime = shiftdata.EndTime,
                                          StartTimeString = shiftdata.StartDate.Date.Add(shiftdata.StartTime).ToString(@"hh\:mm tt"),
                                          EndTimeString = shiftdata.EndDate.Date.Add(shiftdata.EndTime).ToString(@"hh\:mm tt"),
                                          IsAccepted = emShift.IsAccepted,
                                          IsRejected = emShift.IsRejected,
                                          IsShiftCompleted = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id && x.EmployeeId == request.Id).Select(x => x.IsShiftCompleted).FirstOrDefault(),
                                          EmployeeName = emInfo.FirstName + " " + emInfo.LastName,
                                          StatusName = status.CodeDescription
                                      }).Distinct().ToList();
                assignedShifts = assignedShifts.Where(x => x.IsShiftCompleted == false).OrderBy(x => x.StartDate).ToList();
                var totalCount = assignedShifts.Count();
                // assignedShifts = assignedShifts.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
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

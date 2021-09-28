using LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeAssignedShifts;
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
using AutoMapper.QueryableExtensions;
using System.Security.Cryptography.X509Certificates;
using LHSAPI.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeCurrentShifts
{
    public class GetEmployeeCurrentShiftsListHandler : IRequestHandler<GetEmployeeCurrentShiftsQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        private static IConfiguration _configuration;
        public GetEmployeeCurrentShiftsListHandler(LHSDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<ApiResponse> Handle(GetEmployeeCurrentShiftsQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                int hour = Convert.ToInt16(_configuration.GetSection("UTC:Hour").Value);
                hour = -(hour);
                int min = Convert.ToInt32(_configuration.GetSection("UTC:Minutes").Value);
                var localTimeFromUTC = DateTime.UtcNow.AddHours(hour).AddMinutes(min).Date;
                var assignedShifts = (from shiftdata in _dbContext.ShiftInfo
                                      join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                                      //  join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                      join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                                      // join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                      join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                                      join tracker in _dbContext.EmployeeShiftTracker on emShift.ShiftId equals tracker.ShiftId
                                      into gj
                                      from subpet in gj.DefaultIfEmpty()
                                      join location in _dbContext.Location on shiftdata.LocationId equals location.LocationId into gj1
                                      from subpet1 in gj1.DefaultIfEmpty()
                                      where shiftdata.IsDeleted == false && shiftdata.IsActive == true &&
                                      emShift.EmployeeId == request.Id
                                       && ((shiftdata.StartUtcDate.Date == localTimeFromUTC ||
                                       shiftdata.EndUtcDate.Date == localTimeFromUTC)
                                      && ((emShift.IsAccepted == true
                                      && (subpet == null || subpet != null && subpet.IsShiftCompleted == false))
                                       || (emShift.IsAccepted == true && emShift.IsRejected == false && subpet.Id > 0 && subpet.IsShiftCompleted == false)))
                                      // && (subpet != null && subpet.IsShiftCompleted == false)
                                      select new AssignedShiftInfoViewModel
                                      {
                                          Id = shiftdata.Id,
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
                                          IsShiftCompleted = subpet.IsShiftCompleted,
                                          IsLogin = _dbContext.EmployeeShiftTracker.Where(x => x.ShiftId == shiftdata.Id && x.EmployeeId == request.Id).Select(x => x.IsLogin).FirstOrDefault(),
                                          EmployeeId = request.Id,
                                          IsLoginButtonVisible = checkLoginButtonVisible(shiftdata),
                                          IsLogoutButtonVisible = checkLogoutButtonVisible(shiftdata)
                                      }).OrderByDescending(x => x.StartDate).Distinct().ToList();

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
                                           ClientImgURL = _dbContext.ClientPicInfo.Where(x => x.ClientId == clInfo.Id).Select(x => x.Path).FirstOrDefault(),
                                           DateOfBirth = clInfo.DateOfBirth,
                                           Id = clInfo.ClientId
                                       }).OrderByDescending(x => x.ClientName).ToList();
                }




                assignedShifts = assignedShifts.Where(x => x.IsShiftCompleted == false).ToList();
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

        private static bool checkLoginButtonVisible(ShiftInfo shiftData)
        {
            bool isLoginVisible = false;
            try
            {
                int hour = Convert.ToInt16(_configuration.GetSection("UTC:Hour").Value);
                int min = Convert.ToInt32(_configuration.GetSection("UTC:Minutes").Value);
                var difference = DateTime.UtcNow.Subtract(shiftData.StartUtcDate.AddHours(hour).AddMinutes(min));
                //var differenceLogout = DateTime.UtcNow.Subtract(shiftData.EndUtcDate.AddHours(hour).AddMinutes(min));
                if (difference.TotalMinutes >= -15 && difference.TotalMinutes <= 15 /*&& differenceLogout.TotalMinutes <= 0*/)
                {
                    isLoginVisible = true;
                }
                else
                {
                    isLoginVisible = false;
                }
            }
            catch (Exception ex)
            {

            }
            return isLoginVisible;
        }

        private static bool checkLogoutButtonVisible(ShiftInfo shiftData)
        {
            bool isLogoutVisible = false;
            try
            {
                int hour = Convert.ToInt16(_configuration.GetSection("UTC:Hour").Value);
                int min = Convert.ToInt32(_configuration.GetSection("UTC:Minutes").Value);
                var difference = DateTime.UtcNow.Subtract(shiftData.EndUtcDate.AddHours(hour).AddMinutes(min));
                var differenceLogin = DateTime.UtcNow.Subtract(shiftData.StartUtcDate.AddHours(hour).AddMinutes(min));
                if (difference.TotalMinutes <= 15 && (differenceLogin.TotalMinutes >= -15))
                {
                    isLogoutVisible = true;
                }
                else
                {
                    isLogoutVisible = false;
                }
            }
            catch (Exception ex)
            {

            }
            return isLogoutVisible;
        }
    }
}

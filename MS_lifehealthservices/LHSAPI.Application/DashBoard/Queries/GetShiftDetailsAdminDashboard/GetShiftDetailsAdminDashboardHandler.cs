
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using static LHSAPI.Common.Enums.ResponseEnums;

using System.Text.Json;
using LHSAPI.Application.Client.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LHSAPI.Application.DashBoard.Queries.GetShiftDetailsAdminDashboard
{
  public class GetShiftDetailsAdminDashboard : IRequestHandler<GetSchedulesShiftAdminDashboardCommand, ApiResponse>, IRequestHandler<GetOnShiftShiftAdminDashboardCommand, ApiResponse>, IRequestHandler<GetCompleteShiftAdminDashboardCommand, ApiResponse>
  {
    private readonly LHSDbContext _dbContext;

    public GetShiftDetailsAdminDashboard(LHSDbContext dbContext)
    {
      _dbContext = dbContext;

    }

    public async Task<ApiResponse> Handle(GetSchedulesShiftAdminDashboardCommand request, CancellationToken cancellationToken)
    {
      ApiResponse response = new ApiResponse();
      try
      {
       
       var list  = GetShiftList(null);

        if (list == null)
        {
          list = new List<ShiftInfoViewModel>();
        }
       

        response.SuccessWithOutMessage(list);
      }
      catch (Exception ex)
      {
        response.Failed(ex.Message);
      }
      return response;
    }
    public async Task<ApiResponse> Handle(GetOnShiftShiftAdminDashboardCommand request, CancellationToken cancellationToken)
    {
      ApiResponse response = new ApiResponse();
      try
      {

        var list = GetShiftList(false);

        if (list == null)
        {
          list = new List<ShiftInfoViewModel>();
        }


        response.SuccessWithOutMessage(list);
      }
      catch (Exception ex)
      {
        response.Failed(ex.Message);
      }
      return response;
    }
    public async Task<ApiResponse> Handle(GetCompleteShiftAdminDashboardCommand request, CancellationToken cancellationToken)
    {
      ApiResponse response = new ApiResponse();
      try
      {

        var list = GetShiftList(true);

        if (list == null)
        {
          list = new List<ShiftInfoViewModel>();
        }


        response.SuccessWithOutMessage(list);
      }
      catch (Exception ex)
      {
        response.Failed(ex.Message);
      }
      return response;
    }

    private List<ShiftInfoViewModel> GetShiftList(bool? IsCompleted)
    {
      var assignedShifts = (from shiftdata in _dbContext.ShiftInfo
                           
                            join emShift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals emShift.ShiftId
                            join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                            join emInfo in _dbContext.EmployeePrimaryInfo on emShift.EmployeeId equals emInfo.Id
                            join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                            join status in _dbContext.StandardCode on emShift.StatusId equals status.ID
                            join tracker in _dbContext.EmployeeShiftTracker on  shiftdata.Id  equals  tracker.ShiftId 
                            into gj
                            from subpet in gj.DefaultIfEmpty()
                            join location in _dbContext.Location on shiftdata.LocationId equals location.LocationId into loc
                            from subloc in loc
                            where shiftdata.IsDeleted == false && shiftdata.IsActive == true
                            && (shiftdata.StartDate.Date == DateTime.Now.Date || shiftdata.EndDate.Date == DateTime.Now.Date)
                            && ((IsCompleted == false && subpet != null && subpet.IsShiftCompleted == IsCompleted && subpet.IsLogin == true)
                            || (IsCompleted == true && subpet != null && subpet.IsShiftCompleted == IsCompleted)
                            || (IsCompleted == null && subpet == null)
                            )
                            // && (subpet != null && subpet.IsShiftCompleted == false)
                            select new ShiftInfoViewModel
                            {
                              Id = shiftdata.Id,
                              Description = shiftdata.Description,
                              Location = subloc.Name == null ? shiftdata.OtherLocation :subloc.Name,
                              EmployeeName = emInfo.FirstName + " " + emInfo.LastName,
                              StartDate = shiftdata.StartDate.Date,
                              EndDate = shiftdata.EndDate.Date,
                              StartTimeString = shiftdata.StartDate.Date.Add(shiftdata.StartTime).ToString(@"hh\:mm tt"),
                              EndTimeString = shiftdata.EndDate.Date.Add(shiftdata.EndTime).ToString(@"hh\:mm tt"),
                              EmployeeId = emInfo.Id,

                                  }).Distinct().ToList();
            var newlist = assignedShifts.OrderBy(x => x.Id).ToList();
            return newlist;
        }



    }

    public class ShiftInfoViewModel
  {
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string Description { get; set; }
    public string EmployeeName { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string StartTimeString { get; set; }
    public string EndTimeString { get; set; }

  }


}

using LHSAPI.Application.Shift.Models;
using LHSAPI.Application.Shift.Queries.GetAcceptedShifts;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace LHSAPI.Application.Shift.Queries.GetAcceptedShiftsInfo
{
    public class GetAcceptedShiftsInfoCommandHandler : IRequestHandler<GetAcceptedShiftsInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;

        public GetAcceptedShiftsInfoCommandHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse> Handle(GetAcceptedShiftsInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                List<ShiftTrackerViewModel> employeeShiftList = new List<ShiftTrackerViewModel>();
                employeeShiftList = (from shift in _dbContext.EmployeeShiftTracker
                                     join shiftInfo in _dbContext.EmployeeShiftInfo
                                     on shift.ShiftId equals shiftInfo.Id
                                     where shift.EmployeeId == request.EmployeeId && shift.IsActive == true && shift.IsDeleted == false
                                     select new ShiftTrackerViewModel
                                     {
                                         Id = shift.Id,
                                         EmployeeId = shift.EmployeeId,
                                         ClientId = shift.ClientId,
                                         CheckInDate = shift.CheckInDate,
                                         StartTimeString = shift.CheckInTime.Value.ToString(@"hh\:mm"),
                                         EndTimeString = shift.CheckOutTime.HasValue ? shift.CheckOutTime.Value.ToString(@"hh\:mm") : String.Empty,
                                         CheckOutDate = shift.CheckOutDate.Value,
                                         CheckInRemarks = shift.CheckInRemarks,
                                         CheckOutRemarks = shift.CheckOutRemarks,

                                     }).AsQueryable().ToList();
                employeeShiftList = employeeShiftList.OrderByDescending(x => x.Id).Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                if (employeeShiftList.Count > 0)
                {
                    var totalCount = employeeShiftList.Count();
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(employeeShiftList.ToList());
                }
                else
                {
                    response.NotFound();
                }
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }
    }
}

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
using LHSAPI.Application.Shift.Models;

namespace LHSAPI.Application.EmployeeStaff.Queries.GetCheckOutClient
{
    public class GetCheckOutClientHandler : IRequestHandler<GetCheckOutClientQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        public GetCheckOutClientHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse> Handle(GetCheckOutClientQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {


                var checkout = (from shiftdata in _dbContext.ShiftInfo
                                join clShift in _dbContext.ClientShiftInfo on shiftdata.Id equals clShift.ShiftId
                                join clInfo in _dbContext.ClientPrimaryInfo on clShift.ClientId equals clInfo.Id
                                where shiftdata.IsDeleted == false && shiftdata.IsActive == true && shiftdata.Id == request.Id
                                select new ClientShiftInfoViewModel
                                {
                                    Id = clShift.Id,
                                    ClientId = clInfo.Id,
                                    Name = clInfo.FirstName + " " + (clInfo.MiddleName == null ? "" : clInfo.MiddleName) + " " + clInfo.LastName,
                                    IsProgressNotesSubmitted = _dbContext.ProgressNotesList.Where(x => x.ClientId == clInfo.Id && x.ShiftId == request.Id && x.IsActive == true && x.IsDeleted == false).Count(),
                                    IsToDoListSubmitted = _dbContext.ToDoShift.Where(x => x.ShiftId == request.Id && x.IsActive == true && x.IsDeleted == false).Count(),
                                    IsCheckoutCompleted=_dbContext.EmployeeShiftTracker.Where(x=>x.ShiftId==request.Id && x.IsActive==true && x.IsDeleted==false).Select(x=>x.IsShiftCompleted).FirstOrDefault()
                                }).OrderByDescending(x => x.Id).ToList();
                var totalCount = checkout.Count();
                response.Total = totalCount;
                response.SuccessWithOutMessage(checkout);
            }
            catch (Exception ex)
            {

            }
            return response;
        }
    }
}

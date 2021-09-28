using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LHSAPI.Application.Shift.Commands.Create.AddDragDropShift
{
    public class AddDragDropShiftInfoCommandHandler : IRequestHandler<AddDragDropShiftInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly IShiftService _shiftService;
        private readonly ISessionService _ISessionService;
        public AddDragDropShiftInfoCommandHandler(LHSDbContext context, IShiftService shiftService, ISessionService ISessionService)
        {
            _context = context;
            _shiftService = shiftService;
            _ISessionService = ISessionService;
        }
        public async Task<ApiResponse> Handle(AddDragDropShiftInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var _shift = _shiftService.GetShiftDetail(request.ShiftId);
                if (_shift != null)
                {
                    // Delete previous entry       
                    _context.ShiftInfo.Where(x => x.Id == request.ShiftId).ToList().ForEach(x => x.IsDeleted = true);                                        
                    _context.SaveChanges();

                    ShiftInfo _ShiftInfo = new ShiftInfo();
                    _ShiftInfo.Description = _shift.Description;
                    _ShiftInfo.EmployeeCount = 1;
                    _ShiftInfo.ClientCount = 1;
                    _ShiftInfo.StartDate = request.StartDate.Date;                    
                    _ShiftInfo.StartTime = _shift.StartTime;
                    _ShiftInfo.EndDate = request.EndDate.Date;
                    _ShiftInfo.EndTime = _shift.EndTime;
                    _ShiftInfo.StartUtcDate = _ShiftInfo.StartDate.Date.Add(_ShiftInfo.StartTime);
                    _ShiftInfo.EndUtcDate = _ShiftInfo.EndDate.Date.Add(_ShiftInfo.EndTime);
                    _ShiftInfo.IsPublished = _shift.IsPublished;
                    _ShiftInfo.LocationId = _shift.LocationId;
                    _ShiftInfo.OtherLocation = _shift.OtherLocation;
                    _ShiftInfo.ShiftRepeatType = _shift.ShiftRepeatType;
                    // _ShiftInfo.StatusId = _shift.StatusId;
                    _ShiftInfo.Reminder = _shift.Reminder;
                    _ShiftInfo.Duration = _shift.Duration;
                    _ShiftInfo.LocationType = _shift.LocationType;
                    _ShiftInfo.IsDeleted = false;
                    _ShiftInfo.CreatedDate = DateTime.Now;
                    _ShiftInfo.IsActive = true;
                    _ShiftInfo.CreatedById = await _ISessionService.GetUserId();
                    await _context.ShiftInfo.AddAsync(_ShiftInfo);
                    _context.SaveChanges();

                    EmployeeShiftInfo _empShift = new EmployeeShiftInfo();
          _empShift.StatusId = _context.StandardCode.Where(x => x.CodeData == "ShiftStatus" && x.CodeDescription == "Pending").FirstOrDefault().ID;
          _empShift.EmployeeId = request.EmployeeId;
                    _empShift.ShiftId = _ShiftInfo.Id;
                    _empShift.IsDeleted = false;
                    _empShift.CreatedDate = DateTime.Now;
                    _empShift.IsActive = true;
                    _empShift.IsActiveNight = _shift.IsActiveNight;
                    _empShift.IsSleepOver = _shift.IsSleepOver;
                    _empShift.CreatedById = await _ISessionService.GetUserId();
                    await _context.EmployeeShiftInfo.AddAsync(_empShift);

                    ClientShiftInfo _clientShift = new ClientShiftInfo();
                    _clientShift.ClientId = _context.ClientShiftInfo.Where(x => x.ShiftId == _shift.Id).Select(x => x.ClientId).FirstOrDefault();
                    _clientShift.ShiftId = _ShiftInfo.Id;
                    _clientShift.IsDeleted = false;
                    _clientShift.CreatedDate = DateTime.Now;
                    _clientShift.IsActive = true;
                    _clientShift.CreatedById = await _ISessionService.GetUserId();
                    await _context.ClientShiftInfo.AddAsync(_clientShift);
          var ServiceItems = _context.ServiceTypeShiftInfo.Where(x => x.ShiftId == _shift.Id).Select(x => x.ServiceTypeId).ToList();
          foreach (var item in ServiceItems)
          {
            ServiceTypeShiftInfo _ServiceTypeShiftInfo = new ServiceTypeShiftInfo();
            _ServiceTypeShiftInfo.ServiceTypeId = item;
            _ServiceTypeShiftInfo.ShiftId = _ShiftInfo.Id;
            _ServiceTypeShiftInfo.IsDeleted = false;
            _ServiceTypeShiftInfo.CreatedDate = DateTime.Now;
            _ServiceTypeShiftInfo.IsActive = true;
            _ServiceTypeShiftInfo.CreatedById = await _ISessionService.GetUserId();
                        await _context.ServiceTypeShiftInfo.AddAsync(_ServiceTypeShiftInfo);
          }
          _context.SaveChanges();
                    response = response.Success(_ShiftInfo);
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

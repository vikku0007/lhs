
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;

using System.Linq;

using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Shift.Commands.Create.AddShiftToDo
{
    public class AddShiftToDoCommandHandler : IRequestHandler<AddShiftToDoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddShiftToDoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddShiftToDoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (!string.IsNullOrEmpty(request.Description) && request.EmployeeId > 0 && request.ShiftId > 0)
                {
          
                    var ExistUser = _context.ShiftToDo.FirstOrDefault(x => x.Description == request.Description && x.EmployeeId == request.EmployeeId && x.ShiftId == request.ShiftId && x.IsDeleted == false && x.IsActive);
                    if (ExistUser == null)
                    {
                        ShiftToDo _ShiftInfo = new ShiftToDo();
                        _ShiftInfo.Description = request.Description;
                        _ShiftInfo.EmployeeId = request.EmployeeId;
                        _ShiftInfo.ShiftId = request.ShiftId;
                    
                        _ShiftInfo.IsDeleted = false;
                        _ShiftInfo.CreatedDate = DateTime.Now;
                        _ShiftInfo.IsActive = true;
                        _ShiftInfo.CreatedById = await _ISessionService.GetUserId();
                        await _context.ShiftToDo.AddAsync(_ShiftInfo);
            _context.SaveChanges();
          
            response = response.Success(_ShiftInfo);
                    }
                    else
                    {
                        response.AlreadyExist();
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
    }
}

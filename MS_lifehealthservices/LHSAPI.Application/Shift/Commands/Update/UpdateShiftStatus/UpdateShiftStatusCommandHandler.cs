
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

namespace LHSAPI.Application.Shift.Commands.Update.UpdateShiftStatus
{
    public class UpdateShiftStatusCommandHandler : IRequestHandler<UpdateShiftStatusCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
    private readonly IShiftService _ShiftService;

    public UpdateShiftStatusCommandHandler(LHSDbContext context, IShiftService ShiftService)
    {
      _context = context;
      _ShiftService = ShiftService;

    }

        public async Task<ApiResponse> Handle(UpdateShiftStatusCommand request, CancellationToken cancellationToken)
        {
            return  await _ShiftService.UpdateShiftStatus(request.Id, request.StatusId);
        }
    }
}

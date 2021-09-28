
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

namespace LHSAPI.Application.Shift.Commands.Update.UpdateShiftInfo
{
    public class UpdateShiftInfoCommandHandler : IRequestHandler<UpdateShiftInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
    private readonly IShiftService _IShiftService;
        public UpdateShiftInfoCommandHandler(LHSDbContext context, ISessionService ISessionService, IShiftService IShiftService )
        {
            _context = context;
            _ISessionService = ISessionService;
            _IShiftService = IShiftService;
        }

        public async Task<ApiResponse> Handle(UpdateShiftInfoCommand request, CancellationToken cancellationToken)
        {
                 return await  _IShiftService.UpdateShift(request);

        }
    }
}


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

namespace LHSAPI.Application.Shift.Commands.Create.AddShiftTemplate
{
    public class AddShiftTemplateCommandHandler : IRequestHandler<AddShiftTemplateCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
    private readonly IShiftService _IShiftService;

    public AddShiftTemplateCommandHandler(LHSDbContext context, IShiftService IShiftService)
    {
      _context = context;
      _IShiftService = IShiftService;
    }

        public async Task<ApiResponse> Handle(AddShiftTemplateCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (!string.IsNullOrEmpty(request.Name) && request.ShiftId !=null && request.ShiftId.Count > 0)
                {
                  response = await _IShiftService.SaveShiftTemplate(request.Name, request.ShiftId);
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

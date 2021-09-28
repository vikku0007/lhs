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
using static LHSAPI.Common.Enums.ResponseEnums;
using LHSAPI.Application.Employee.Models;
using LHSAPI.Application.Interface;

namespace LHSAPI.Application.Administration.Commands.Create.AddToDoListItem
{
    public class AddToDoListItemHandler : IRequestHandler<AddToDoListItemCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddToDoListItemHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddToDoListItemCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {

                var ExistUser = _context.ToDoShiftItem.FirstOrDefault(x => x.ShiftType == request.ShiftType && x.Description == request.Description && x.IsActive == true);
                if (ExistUser == null)
                {
                    LHSAPI.Domain.Entities.ToDoShiftItem shift = new LHSAPI.Domain.Entities.ToDoShiftItem();

                    shift.ShiftType = request.ShiftType;
                    shift.Description = request.Description;
                    shift.CreatedById = await _ISessionService.GetUserId();
                    shift.CreatedDate = DateTime.Now;
                    shift.IsActive = true;
                    await _context.ToDoShiftItem.AddAsync(shift);
                    _context.SaveChanges();
                    response.Success(shift);

                }
                else
                {
                    response.AlreadyExist();

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

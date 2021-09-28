using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteEmployeeLeave
{

    public class DeleteEmployeeLeaveCommandHandler : IRequestHandler<DeleteEmployeeLeaveCommand, ApiResponse>
    {

        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public DeleteEmployeeLeaveCommandHandler(LHSDbContext context, ISessionService ISessionService)
        { 
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(DeleteEmployeeLeaveCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var leaveResult = _context.EmployeeLeaveInfo.FirstOrDefault(x => x.Id == request.Id &&  x.IsDeleted == false && x.IsActive == true);
                    if (leaveResult == null)
                    {
                        response.NotFound();
                    }
                    else
                    {
                        leaveResult.IsDeleted = true;
                        leaveResult.IsActive = false;
                        leaveResult.DeletedDate = DateTime.UtcNow;
                        leaveResult.DeletedById = await _ISessionService.GetUserId();
                        _context.EmployeeLeaveInfo.Update(leaveResult);
                        await _context.SaveChangesAsync();
                        response.Delete(leaveResult);

                    }
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

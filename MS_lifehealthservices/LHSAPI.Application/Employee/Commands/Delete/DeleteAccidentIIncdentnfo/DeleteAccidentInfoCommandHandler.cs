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

namespace LHSAPI.Application.Employee.Commands.Delete.DeleteAccidentIncidentInfo
{

    public class DeleteAccidentInfoCommandHandler : IRequestHandler<DeleteAccidentInfoCommand, ApiResponse>
    { 

        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public DeleteAccidentInfoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(DeleteAccidentInfoCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var ExistEmp = _context.EmployeeAccidentInfo.FirstOrDefault(x => x.Id == request.Id &&  x.IsDeleted == false && x.IsActive == true);
                    if (ExistEmp == null)
                    {
                        response.NotFound();
                    }
                    else
                    {
                       
                        ExistEmp.IsDeleted = true;
                        ExistEmp.IsActive = false;
                        ExistEmp.DeletedDate = DateTime.UtcNow;
                        ExistEmp.DeletedById = await _ISessionService.GetUserId();
                        _context.EmployeeAccidentInfo.Update(ExistEmp);
                        await _context.SaveChangesAsync();
                        response.Delete(ExistEmp);

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

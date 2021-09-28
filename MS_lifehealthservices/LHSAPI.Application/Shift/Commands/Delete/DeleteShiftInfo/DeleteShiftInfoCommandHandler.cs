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

namespace LHSAPI.Application.Shift.Commands.Delete.DeleteShiftInfo
{
    public class DeleteShiftInfoCommandHandler : IRequestHandler<DeleteShiftInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public DeleteShiftInfoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(DeleteShiftInfoCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var ExistEmp = _context.ShiftInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                      
                    if (ExistEmp == null)
                    {
                        response.NotFound();
                    }
                    else
                    {
              //          var EmployeeShiftInfo = _context.EmployeeShiftInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
              //          var ClientShiftInfo = _context.ClientShiftInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                     
              //          if (EmployeeShiftInfo != null)
              //          {
              //EmployeeShiftInfo.IsDeleted = true;
              //EmployeeShiftInfo.IsActive = false;
              //EmployeeShiftInfo.DeletedDate = DateTime.UtcNow;
              //EmployeeShiftInfo.DeletedById = 1;
              //              _context.EmployeeShiftInfo.Update(EmployeeShiftInfo);
              //          }
              //          if (ClientShiftInfo != null)
              //          {
              //ClientShiftInfo.IsDeleted = true;
              //ClientShiftInfo.IsActive = false;
              //ClientShiftInfo.DeletedDate = DateTime.UtcNow;
              //ClientShiftInfo.DeletedById = 1;
              //              _context.ClientShiftInfo.Update(ClientShiftInfo);
              //          }
                        
                        

                        ExistEmp.IsDeleted = true;
                        ExistEmp.IsActive = false;
                        ExistEmp.DeletedDate = DateTime.UtcNow;
                        ExistEmp.DeletedById = await _ISessionService.GetUserId();
                       _context.ShiftInfo.Update(ExistEmp);
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

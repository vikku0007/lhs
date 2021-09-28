
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using LHSAPI.Application.Interface;
using System.IO;

namespace LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateDayReportSupportWorker
{
    public class UpdateSupportWorkerHandler : IRequestHandler<UpdateSupportWorkerCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
       
        public UpdateSupportWorkerHandler(LHSDbContext context,ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
          

        }

        public async Task<ApiResponse> Handle(UpdateSupportWorkerCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var ExistUser = _context.DayReportSupportWorkers.FirstOrDefault(x => x.Id == request.Id && x.IsActive == true);
                    if (ExistUser != null)
                    {
                       
                        ExistUser.StaffName = request.StaffName;
                        ExistUser.TimeIn = request.TimeIn;
                        ExistUser.TimeOut = request.TimeOut;
                        ExistUser.ShiftId = request.ShiftId;
                        ExistUser.Signature = request.Signature;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                       _context.DayReportSupportWorkers.Update(ExistUser);
                        await _context.SaveChangesAsync();
                        response.Update(ExistUser);
                    }
                    else
                    {
                        response.NotFound();
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

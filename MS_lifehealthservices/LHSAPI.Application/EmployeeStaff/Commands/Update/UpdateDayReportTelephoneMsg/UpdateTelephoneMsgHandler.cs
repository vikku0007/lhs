
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
using System.IO;

namespace LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateDayReportTelephoneMsg
{
    public class UpdateTelephoneMsgHandler : IRequestHandler<UpdateTelephoneMsgCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        
        public UpdateTelephoneMsgHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
            

        }

        public async Task<ApiResponse> Handle(UpdateTelephoneMsgCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var ExistUser = _context.DayReportTelePhoneMsg.FirstOrDefault(x => x.Id == request.Id && x.IsActive == true);
                    if (ExistUser != null)
                    {
                        
                        ExistUser.Time = request.Time;
                        ExistUser.ShiftId = request.ShiftId;
                        ExistUser.Caller = request.Caller;
                        ExistUser.MessageFor = request.MessageFor;
                        ExistUser.Message = request.Message;
                        ExistUser.Signature = request.Signature;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        _context.DayReportTelePhoneMsg.Update(ExistUser);
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

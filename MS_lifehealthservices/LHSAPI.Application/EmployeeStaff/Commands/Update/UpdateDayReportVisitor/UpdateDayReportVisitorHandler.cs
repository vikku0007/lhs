
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

namespace LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateDayReportVisitor
{
    public class UpdateDayReportVisitorHandler : IRequestHandler<UpdateDayReportVisitorCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public UpdateDayReportVisitorHandler(LHSDbContext context, IHostingEnvironment hostingEnvironment, IConfiguration configuration, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;

        }

        public async Task<ApiResponse> Handle(UpdateDayReportVisitorCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var ExistUser = _context.DayReportVisitor.FirstOrDefault(x => x.Id == request.Id && x.IsActive == true);
                    if (ExistUser != null)
                    {
                        ExistUser.Time = request.Time;
                        ExistUser.ShiftId = request.ShiftId;
                        ExistUser.VisitorName = request.VisitorName;
                        ExistUser.VisitReason = request.VisitReason;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        _context.DayReportVisitor.Update(ExistUser);
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

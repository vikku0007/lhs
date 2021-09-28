
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

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportVisitor
{
    public class AddDayReportVisitorHandler : IRequestHandler<AddDayReportVisitorCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public AddDayReportVisitorHandler(LHSDbContext context, IHostingEnvironment hostingEnvironment, IConfiguration configuration, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;

        }

        public async Task<ApiResponse> Handle(AddDayReportVisitorCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ShiftId > 0)
                {

                    var ExistUser = _context.DayReportVisitor.FirstOrDefault(x => x.ShiftId == request.ShiftId && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        DayReportVisitor _DayReportVisitor = new DayReportVisitor();
                        _DayReportVisitor.Time = request.Time;
                        _DayReportVisitor.ShiftId = request.ShiftId;
                        _DayReportVisitor.VisitorName = request.VisitorName;
                        _DayReportVisitor.VisitReason = request.VisitReason;
                        _DayReportVisitor.CreatedById = await _ISessionService.GetUserId();
                        _DayReportVisitor.CreatedDate = DateTime.Now;
                        _DayReportVisitor.IsActive = true;
                        _DayReportVisitor.IsDeleted = false;
                        await _context.DayReportVisitor.AddAsync(_DayReportVisitor);
                        _context.SaveChanges();
                        response.Success(_DayReportVisitor);

                    }
                    else
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

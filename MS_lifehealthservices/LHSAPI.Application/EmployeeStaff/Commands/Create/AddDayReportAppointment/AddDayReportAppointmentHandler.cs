
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

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportAppointment
{
    public class AddDayReportAppointmentHandler : IRequestHandler<AddDayReportAppointmentCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public AddDayReportAppointmentHandler(LHSDbContext context, IHostingEnvironment hostingEnvironment, IConfiguration configuration, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;

        }

        public async Task<ApiResponse> Handle(AddDayReportAppointmentCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ShiftId > 0)
                {

                    var ExistUser = _context.DayReportAppointments.FirstOrDefault(x => x.ShiftId == request.ShiftId && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        DayReportAppointments _DayReportVisitor = new DayReportAppointments();
                        _DayReportVisitor.ClientId = request.ClientId;
                        _DayReportVisitor.ShiftId = request.ShiftId;
                        _DayReportVisitor.Details = request.Details;
                        _DayReportVisitor.Time = request.Time;
                        _DayReportVisitor.GeneralInformation = request.GeneralInformation;
                        _DayReportVisitor.NightReport = request.NightReport;
                        _DayReportVisitor.CreatedById = await _ISessionService.GetUserId();
                        _DayReportVisitor.CreatedDate = DateTime.Now;
                        _DayReportVisitor.IsActive = true;
                        _DayReportVisitor.IsDeleted = false;
                        await _context.DayReportAppointments.AddAsync(_DayReportVisitor);
                        _context.SaveChanges();
                        response.Success(_DayReportVisitor);

                    }
                    else
                    {
                        ExistUser.Details = request.Details;
                        ExistUser.Time = request.Time;
                        ExistUser.GeneralInformation = request.GeneralInformation;
                        ExistUser.NightReport = request.NightReport;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        _context.DayReportAppointments.Update(ExistUser);
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

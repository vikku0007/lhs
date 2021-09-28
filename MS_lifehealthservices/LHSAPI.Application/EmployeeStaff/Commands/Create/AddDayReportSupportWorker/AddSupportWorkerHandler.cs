
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

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportSupportWorker
{
    public class AddSupportWorkerHandler : IRequestHandler<AddSupportWorkerCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public AddSupportWorkerHandler(LHSDbContext context, IHostingEnvironment hostingEnvironment, IConfiguration configuration, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;

        }

        public async Task<ApiResponse> Handle(AddSupportWorkerCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ShiftId > 0)
                {

                    var ExistUser = _context.DayReportSupportWorkers.FirstOrDefault(x => x.ShiftId == request.ShiftId && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        DayReportSupportWorkers _DayReportSupportWorkers = new DayReportSupportWorkers();
                        _DayReportSupportWorkers.StaffName = request.StaffName;
                        _DayReportSupportWorkers.TimeIn = request.TimeIn;
                        _DayReportSupportWorkers.TimeOut = request.TimeOut;
                        _DayReportSupportWorkers.CreatedById = await _ISessionService.GetUserId();
                        _DayReportSupportWorkers.CreatedDate = DateTime.Now;
                        _DayReportSupportWorkers.IsActive = true;
                        _DayReportSupportWorkers.IsDeleted = false;
                        _DayReportSupportWorkers.ShiftId = request.ShiftId;
                        _DayReportSupportWorkers.Signature = request.Signature;
                        if (request.Signature.Length > 0)
                        await _context.DayReportSupportWorkers.AddAsync(_DayReportSupportWorkers);
                        _context.SaveChanges();
                        response.Success(_DayReportSupportWorkers);

                    }
                    else
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

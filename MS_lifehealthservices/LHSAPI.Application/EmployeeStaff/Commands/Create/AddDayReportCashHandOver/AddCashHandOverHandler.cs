
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

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddDayReportCashHandOver
{
    public class AddCashHandOverHandler : IRequestHandler<AddCashHandOverCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public AddCashHandOverHandler(LHSDbContext context, IHostingEnvironment hostingEnvironment, IConfiguration configuration, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;

        }

        public async Task<ApiResponse> Handle(AddCashHandOverCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ShiftId > 0)
                {

                    var ExistUser = _context.DayReportCashHandOver.FirstOrDefault(x => x.ShiftId == request.ShiftId && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        DayReportCashHandOver _DayReportVisitor = new DayReportCashHandOver();
                        _DayReportVisitor.CashHandover = request.CashHandover;
                        _DayReportVisitor.ShiftId = request.ShiftId;
                        _DayReportVisitor.BalanceaBroughtAM = request.BalanceaBroughtAM;
                        _DayReportVisitor.ExpenseAM = request.ExpenseAM;
                        _DayReportVisitor.BalanceaBroughtPM = request.BalanceaBroughtPM;
                        _DayReportVisitor.ExpensePM = request.ExpensePM;
                        _DayReportVisitor.Signature = request.Signature;
                        _DayReportVisitor.CreatedById = await _ISessionService.GetUserId();
                        _DayReportVisitor.CreatedDate = DateTime.Now;
                        _DayReportVisitor.IsActive = true;
                        _DayReportVisitor.IsDeleted = false;
                        await _context.DayReportCashHandOver.AddAsync(_DayReportVisitor);
                        _context.SaveChanges();
                        response.Success(_DayReportVisitor);

                    }
                    else
                    {
                        ExistUser.CashHandover = request.CashHandover;
                        ExistUser.ShiftId = request.ShiftId;
                        ExistUser.BalanceaBroughtAM = request.BalanceaBroughtAM;
                        ExistUser.ExpenseAM = request.ExpenseAM;
                        ExistUser.BalanceaBroughtPM = request.BalanceaBroughtPM;
                        ExistUser.ExpensePM = request.ExpensePM;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        _context.DayReportCashHandOver.Update(ExistUser);
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

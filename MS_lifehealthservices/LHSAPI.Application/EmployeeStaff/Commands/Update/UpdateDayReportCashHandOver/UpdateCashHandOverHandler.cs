
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

namespace LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateDayReportCashHandOver
{
    public class UpdateCashHandOverHandler : IRequestHandler<UpdateCashHandOverCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public UpdateCashHandOverHandler(LHSDbContext context, IHostingEnvironment hostingEnvironment, IConfiguration configuration, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;

        }

        public async Task<ApiResponse> Handle(UpdateCashHandOverCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var ExistUser = _context.DayReportCashHandOver.FirstOrDefault(x => x.Id == request.Id && x.IsActive == true);
                    if (ExistUser != null)
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

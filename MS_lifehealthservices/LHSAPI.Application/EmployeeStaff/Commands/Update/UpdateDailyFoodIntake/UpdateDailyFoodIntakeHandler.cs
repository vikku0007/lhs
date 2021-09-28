
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

namespace LHSAPI.Application.EmployeeStaff.Commands.Update.UpdateDailyFoodIntake
{
    public class UpdateDailyFoodIntakeHandler : IRequestHandler<UpdateDailyFoodIntakeCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public UpdateDailyFoodIntakeHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
           
        }

        public async Task<ApiResponse> Handle(UpdateDailyFoodIntakeCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var ExistUser = _context.DayReportFoodIntake.FirstOrDefault(x => x.Id == request.Id && x.IsActive == true);
                    if (ExistUser != null)
                    {
                       
                        ExistUser.StaffName = request.StaffName;
                        ExistUser.ResidentName = request.ResidentName;
                        ExistUser.Date = request.Date;
                        ExistUser.Breakfast = request.Breakfast;
                        ExistUser.Lunch = request.Lunch;
                        ExistUser.Dinner = request.Dinner;
                        ExistUser.Snacks = request.Snacks;
                        ExistUser.fluids = request.fluids;
                        ExistUser.ShiftId = request.ShiftId;
                        ExistUser.Signature = request.Signature;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                       _context.DayReportFoodIntake.Update(ExistUser);
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

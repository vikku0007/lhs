
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

namespace LHSAPI.Application.EmployeeStaff.Commands.Create.AddDailyFoodIntake
{
    public class AddDailyFoodIntakeHandler : IRequestHandler<AddDailyFoodIntakeCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public AddDailyFoodIntakeHandler(LHSDbContext context, IHostingEnvironment hostingEnvironment, IConfiguration configuration, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;

        }

        public async Task<ApiResponse> Handle(AddDailyFoodIntakeCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ShiftId > 0)
                {

                    var ExistUser = _context.DayReportFoodIntake.FirstOrDefault(x => x.ShiftId == request.ShiftId && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        DayReportFoodIntake _DayReportFoodIntake = new DayReportFoodIntake();
                        _DayReportFoodIntake.ResidentName = request.ResidentName;
                        _DayReportFoodIntake.Date = request.Date;
                        _DayReportFoodIntake.StaffName = request.StaffName;
                        _DayReportFoodIntake.Breakfast = request.Breakfast;
                        _DayReportFoodIntake.Lunch = request.Lunch;
                        _DayReportFoodIntake.Dinner = request.Dinner;
                        _DayReportFoodIntake.Snacks = request.Snacks;
                        _DayReportFoodIntake.fluids = request.fluids;
                        _DayReportFoodIntake.CreatedById = await _ISessionService.GetUserId();
                        _DayReportFoodIntake.CreatedDate = DateTime.Now;
                        _DayReportFoodIntake.IsActive = true;
                        _DayReportFoodIntake.IsDeleted = false;
                        _DayReportFoodIntake.ShiftId = request.ShiftId;
                        _DayReportFoodIntake.Signature = request.Signature;
                        if (request.Signature.Length > 0)
                        await _context.DayReportFoodIntake.AddAsync(_DayReportFoodIntake);
                        _context.SaveChanges();
                        response.Success(_DayReportFoodIntake);

                    }
                    else
                    {
                        ExistUser.StaffName = request.StaffName;
                        ExistUser.ResidentName = request.ResidentName;
                        ExistUser.StaffName = request.StaffName;
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

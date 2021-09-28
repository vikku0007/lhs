using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeAvailability
{
    public class AddEmployeeAvailabilityCommandHandler : IRequestHandler<AddEmployeeAvailabilityCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public AddEmployeeAvailabilityCommandHandler(LHSDbContext context)
        {
            _context = context;

        }

        public async Task<ApiResponse> Handle(AddEmployeeAvailabilityCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {

                    var ExistUser = _context.EmployeeAvailabilityDetails.FirstOrDefault(x => x.EmployeeId == request.EmployeeId && x.AvailabilityDay==request.AvailabilityDay && x.StartTime == request.StartTime && x.EndTime == request.EndTime && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        EmployeeAvailabilityDetails user = new EmployeeAvailabilityDetails();
                        user.EmployeeId = request.EmployeeId;
                        user.AvailabilityDay = request.AvailabilityDay;
                        user.StartTime = request.StartTime;
                        user.EndTime = request.EndTime;
                        user.CreatedById = 1;
                        user.CreatedDate = DateTime.Now;
                        user.WorksMon = request.WorksMon;
                        user.IsActive = true;
                        await _context.EmployeeAvailabilityDetails.AddAsync(user);
                        _context.SaveChanges();
                        response.Status = (int)Number.One;
                        response.ResponseData = user;
                        response.Message = ResponseMessage.Success;

                    }
                    else
                    {
                        response.Status = (int)Number.Zero;
                        response.Message = ResponseMessage.Exist;

                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                throw ex;

                //response.Status = (int)Number.Zero;
                //response.Message = ResponseMessage.Error;

            }
            return response;

        }
    }
}

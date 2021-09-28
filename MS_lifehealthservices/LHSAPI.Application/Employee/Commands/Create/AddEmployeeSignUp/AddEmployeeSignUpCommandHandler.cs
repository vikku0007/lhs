using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeSignUp
{
  public class AddEmployeeSignUpCommandHandler : IRequestHandler<AddEmployeeSignUpCommand, ApiResponse>
  {
    private readonly LHSDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AddEmployeeSignUpCommandHandler(LHSDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
      _context = context;
      _userManager = userManager;
      _roleManager = roleManager;
    }

    public async Task<ApiResponse> Handle(AddEmployeeSignUpCommand request, CancellationToken cancellationToken)
    {
      ApiResponse response = new ApiResponse();
      try
      {
        if (!string.IsNullOrEmpty(request.Password))
        {
          var ExistUser = _context.EmployeePrimaryInfo.FirstOrDefault(x => x.Id == request.EmployeeId & x.IsActive == true & x.IsDeleted == false);
          if (ExistUser != null && !string.IsNullOrEmpty(ExistUser.EmailId))
          {
            var isExist = await _userManager.FindByEmailAsync(ExistUser.EmailId);
            if (isExist != null)
            {

              var code = await _userManager.GeneratePasswordResetTokenAsync(isExist);
              var isUpdated = await _userManager.ResetPasswordAsync(isExist, code, request.Password);
              if (isUpdated.Succeeded)
              {
                response.Success();
              }
              else
              {
                response.Failed("Password not updated");
              }

            }
            else
            {
              response.NotFound();

            }

          }
          else
          {
            response.NotFound();

          }
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




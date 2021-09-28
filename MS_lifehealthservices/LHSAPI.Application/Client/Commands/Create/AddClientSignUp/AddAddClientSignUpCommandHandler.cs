using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Common.CommonMethods;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Employee.Commands.Create.AddClientSignUp
{
  public class AddClientSignUpCommandHandler : IRequestHandler<AddClientSignUpCommand, ApiResponse>
  {
    private readonly LHSDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IMessageService _IMessageService;


    public AddClientSignUpCommandHandler(LHSDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMessageService IMessageService)
    {
      _context = context;
      _userManager = userManager;
      _roleManager = roleManager;
      _configuration = configuration;
      _IMessageService = IMessageService;
    }

    public async Task<ApiResponse> Handle(AddClientSignUpCommand request, CancellationToken cancellationToken)
    {
      ApiResponse response = new ApiResponse();
      try
      {
        if (request.ClientId > 0)
        {
          var ExistUser = _context.ClientPrimaryInfo.FirstOrDefault(x => x.Id == request.ClientId & x.IsActive == true & x.IsDeleted == false);
          if (ExistUser != null && !string.IsNullOrEmpty(ExistUser.FirstName))
          {
            _context.Database.BeginTransaction();
            string UserName = ExistUser.FirstName + CommonFunction.Get4DigitID();
            var newUser = new ApplicationUser
            {
              UserName = UserName,
              FirstName = ExistUser.FirstName,
              LastName = ExistUser.LastName,
              PhoneNumber = ExistUser.MobileNo,
             // Email = ExistUser.EmailId,
              PhoneNumberConfirmed = true,
              EmailConfirmed = true,
          //    EmployeeId = ExistUser.Id,
              IsClient = true,
              ClientId = ExistUser.Id,
             // NormalizedEmail = ExistUser.EmailId,
             NormalizedUserName = UserName
            };

            if(!string.IsNullOrEmpty(request.EmailId))
            {
              newUser.UserName = request.EmailId;
              newUser.NormalizedUserName = request.EmailId;
              newUser.NormalizedEmail = request.EmailId;
              newUser.Email = request.EmailId;
              ExistUser.EmailId = request.EmailId;
              ExistUser.UserName = request.EmailId;
              _context.Update(ExistUser);
              _context.SaveChanges();
            }
            else
            {
              ExistUser.UserName = UserName;
              _context.Update(ExistUser);
              _context.SaveChanges();
            }

            var objNew = await _userManager.CreateAsync(newUser, ExistUser.FirstName.ToLower() + "@123");
            if (objNew.Succeeded)
            {
              var s = await _roleManager.CreateAsync(new IdentityRole(UserRole.Employee.ToString()));
              var id = newUser.Id;
              await _userManager.AddClaimAsync(newUser, new Claim("Roles", UserRole.Employee.ToString()));
              await _userManager.AddToRoleAsync(newUser, UserRole.Employee.ToString());
              await _context.SaveChangesAsync();
              User user1 = new User();
              user1.UserName = newUser.UserName;
              user1.UserById = newUser.Id;
              user1.PhoneNo = ExistUser.MobileNo;
              //user.SSN = existrecord.SSN;
              user1.isLoginFirstTime = true;
              user1.Lat = 0;
              user1.Long = 0;

              await _context.User.AddAsync(user1);
              _context.SaveChanges();
             
              response.Success(ExistUser);
              _context.Database.CommitTransaction();
              string emailBody = _IMessageService.GetEmailTemplate();
              string Message = "Welcome to LHS you are successfully registered with us";
              string Subject = "Successfully Registered!";

              string Password = ExistUser.FirstName + "@123";
              emailBody = emailBody.Replace("{Message}", Message);
              emailBody = emailBody.Replace("{Subject}", Subject);
              emailBody = emailBody.Replace("{UserName}", UserName);
              emailBody = emailBody.Replace("{Email}", ExistUser.EmailId);
              emailBody = emailBody.Replace("{Password}", Password);
              _IMessageService.SendingEmails(ExistUser.EmailId, Subject, emailBody);
            }
            else
            {
              _context.Database.RollbackTransaction();
              response.Status = (int)Number.Zero;
              response.Message = ResponseMessage.Error;
              response.ResponseData = null;
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
        _context.Database.RollbackTransaction();
        response.Failed(ex.Message);

      }
      return response;

    }

    public async Task<ApiResponse> Handle(ClientResetPasswordCommand request, CancellationToken cancellationToken)
    {
      ApiResponse response = new ApiResponse();
      try
      {
        if (!string.IsNullOrEmpty(request.Password))
        {
          var ExistUser = _context.ClientPrimaryInfo.FirstOrDefault(x => x.Id == request.ClientId & x.IsActive == true & x.IsDeleted == false);
          if (ExistUser != null)
          {
            ApplicationUser luser = null;
            if (!string.IsNullOrEmpty(ExistUser.EmailId))
            {
              luser = await _userManager.FindByEmailAsync(ExistUser.EmailId);
              if(luser == null)
              {
                luser = await _userManager.FindByNameAsync(ExistUser.UserName);
              }
            }
            
           
            if (luser != null)
            {

              var code = await _userManager.GeneratePasswordResetTokenAsync(luser);
              var isUpdated = await _userManager.ResetPasswordAsync(luser, code, request.Password);
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




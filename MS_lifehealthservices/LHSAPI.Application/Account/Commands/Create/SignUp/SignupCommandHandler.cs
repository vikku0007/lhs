
using MediatR;
using Microsoft.AspNetCore.Identity;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Account.Commands.Create.SignUp
{
    public class SignupCommandHandler : IRequestHandler<SignupCommand, ApiResponse>
    {

        private readonly LHSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SignupCommandHandler(LHSDbContext context, UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<ApiResponse> Handle (SignupCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
              //  if (!string.IsNullOrEmpty(request.UserName))
              //  {

              //      var ExistUser = _context.UserRegister.FirstOrDefault(x => x.UserName == request.UserName && x.IsActive == true);
              //      if (ExistUser == null)
              //      {
              //          var userexist = _context.User.FirstOrDefault(x => x.UserName == request.UserName);
              //          if (userexist == null)
              //          {

              //              var newUser = new ApplicationUser {
              //                  UserName = request.Email,
              //                  FirstName = request.FirstName, 
              //                  LastName = request.LastName, 
              //                  PhoneNumber = request.PhoneNo,
              //                  Email = request.Email,
              //                  PhoneNumberConfirmed = true,
              //                  EmailConfirmed = false
              //              };
              //var objNew = await _userManager.CreateAsync(newUser, request.Password);
              //if (objNew.Succeeded)
              //{
              //  var s = await _roleManager.CreateAsync(new IdentityRole(UserRole.Client.ToString()));
              //  var id = newUser.Id;
              //  await _userManager.AddClaimAsync(newUser, new Claim("Roles", UserRole.Client.ToString()));
              //  await _userManager.AddToRoleAsync(newUser, UserRole.Client.ToString());

              //  User user = new User();
              //  user.UserName = request.UserName;
              //  user.UserById = newUser.Id;
              //  user.PhoneNo = request.PhoneNo;
              //  //user.SSN = existrecord.SSN;
              //  user.isLoginFirstTime = true;
              //  user.Lat = 0;
              //  user.Long = 0;
               
              //  await _context.User.AddAsync(user);
              //  _context.SaveChanges();

              //  //ExistUser.IsActive = true;
              //  //ExistUser.UpdatedDate = DateTime.UtcNow;
              //  //_context.UserRegister.Update(ExistUser);
              //  await _context.SaveChangesAsync();
              //  response.Status = (int)Number.One;
              //  response.Message = ResponseMessage.Success;
              //  response.ResponseData = user;
              //}
              //else
              //{
              //  response.Status = (int)Number.Zero;
              //  response.Message = ResponseMessage.Error;
              //  response.ResponseData = null;
              //}

              //          }
              //          else
              //          {
              //              response.Status = (int)Number.One;
              //              response.Message = ResponseMessage.UserExist;
              //          }


              //      }
              //      else
              //      {
              //          response.Status = (int)Number.One;
              //          response.Message = ResponseMessage.UserExist;

              //      }
              //  }
              //  else
              //  {
              //      var existrecord = _context.UserRegister.FirstOrDefault(x => x.Id == request.Id);
              //      if (existrecord != null)
              //      {
              //          existrecord.FirstName = request.FirstName;
              //          existrecord.LastName = request.LastName;
              //          existrecord.MiddleName = request.MiddleName;
              //          //existrecord.EmailId = request.EmailId;
              //       //   existrecord.Password = request.Password;
              //          existrecord.UpdatedDate = DateTime.UtcNow;
              //          existrecord.PhoneNo = request.PhoneNo;
              //          //existrecord.OTP = number;
              //          //existrecord.OTPStartDateTime = DateTime.UtcNow;
              //          //existrecord.OTPEndDateTime = DateTime.UtcNow.AddMinutes(5);

              //          _context.Update(existrecord);
              //          await _context.SaveChangesAsync();
              //          //SendOTPMessage.SendMessage(request.PhoneNo, number);

              //          response.Status = (int)Number.One;
              //          response.Message = ResponseMessage.Success;
              //          response.ResponseData = existrecord;
              //      }
              //      else
              //      {
              //          response.Status = (int)Number.Zero;
              //          response.Message = ResponseMessage.PhoneExist;
              //      }

              //  }
                
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

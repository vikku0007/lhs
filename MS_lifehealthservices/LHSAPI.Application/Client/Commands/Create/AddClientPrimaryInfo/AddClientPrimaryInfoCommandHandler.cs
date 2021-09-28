
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;

using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Client.Commands.Create.AddClientPrimaryInfo
{
    public class AddClientPrimaryInfoCommandHandler : IRequestHandler<AddClientPrimaryInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private readonly IMessageService _IMessageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AddClientPrimaryInfoCommandHandler(LHSDbContext context, ISessionService ISessionService, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMessageService IMessageService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _ISessionService = ISessionService;
            _IMessageService = IMessageService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<ApiResponse> Handle(AddClientPrimaryInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (!string.IsNullOrEmpty(request.FirstName))
                {

                    var ExistUser = _context.ClientPrimaryInfo.FirstOrDefault(x => x.FirstName == request.FirstName & x.IsActive == true);
                    if (ExistUser == null)
                    {
                        ClientPrimaryInfo _ClientPrimaryInfo = new ClientPrimaryInfo();
                        _ClientPrimaryInfo.FirstName = request.FirstName;
                        _ClientPrimaryInfo.LastName = request.LastName;
                        _ClientPrimaryInfo.CreatedById = await _ISessionService.GetUserId();
                        _ClientPrimaryInfo.ClientId = Common.CommonMethods.CommonFunction.Get8DigitID();
                        _ClientPrimaryInfo.CreatedDate = DateTime.Now;
                        _ClientPrimaryInfo.Gender = request.Gender;
                        _ClientPrimaryInfo.IsActive = true;
                        //  _ClientPrimaryInfo.ClientId = request.ClientId;
                        _ClientPrimaryInfo.DateOfBirth = request.DateOfBirth;
                        _ClientPrimaryInfo.EmailId = request.EmailId;
                        _ClientPrimaryInfo.MaritalStatus = request.MaritalStatus;
                        _ClientPrimaryInfo.Salutation = request.Salutation;
                        _ClientPrimaryInfo.LocationId = request.LocationId;
                        _ClientPrimaryInfo.MiddleName = request.MiddleName;
                        _ClientPrimaryInfo.MobileNo = request.MobileNo;
                        _ClientPrimaryInfo.EmployeeId = request.EmployeeId;
                        _ClientPrimaryInfo.Address = request.Address;
                        _ClientPrimaryInfo.LocationType = request.LocationType;
                        _ClientPrimaryInfo.OtherLocation = request.OtherLocation;
                        _ClientPrimaryInfo.IsActive = true;
                        _ClientPrimaryInfo.IsDeleted = false;
                        await _context.ClientPrimaryInfo.AddAsync(_ClientPrimaryInfo);
                        _context.SaveChanges();
                        response.Success(_ClientPrimaryInfo);
                        //var newUser = new ApplicationUser
                        //{
                        //    UserName = _ClientPrimaryInfo.EmailId,
                        //    FirstName = _ClientPrimaryInfo.FirstName,
                        //    LastName = _ClientPrimaryInfo.LastName,
                        //    PhoneNumber = _ClientPrimaryInfo.MobileNo,
                        //    Email = _ClientPrimaryInfo.EmailId,
                        //    PhoneNumberConfirmed = true,
                        //    EmailConfirmed = false,
                        //    EmployeeId = _ClientPrimaryInfo.Id,
                        //    NormalizedEmail = _ClientPrimaryInfo.EmailId,
                        //    NormalizedUserName = _ClientPrimaryInfo.EmailId
                        //};
                        //var objNew = await _userManager.CreateAsync(newUser, _ClientPrimaryInfo.FirstName.ToLower() + "@123");
                        //if (objNew.Succeeded)
                        //{
                        //    var s = await _roleManager.CreateAsync(new IdentityRole(UserRole.Employee.ToString()));
                        //    var id = newUser.Id;
                        //    await _userManager.AddClaimAsync(newUser, new Claim("Roles", UserRole.Employee.ToString()));
                        //    await _userManager.AddToRoleAsync(newUser, UserRole.Employee.ToString());
                        //    await _context.SaveChangesAsync();
                        //    User user1 = new User();
                        //    user1.UserName = _ClientPrimaryInfo.EmailId;
                        //    user1.UserById = newUser.Id;
                        //    user1.PhoneNo = _ClientPrimaryInfo.MobileNo;
                        //    //user.SSN = existrecord.SSN;
                        //    user1.isLoginFirstTime = true;
                        //    user1.Lat = 0;
                        //    user1.Long = 0;
                        //    await _context.User.AddAsync(user1);
                        //    _context.SaveChanges();
                        //    var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                        //    token = System.Web.HttpUtility.UrlEncode(token);
                        //    string url = _configuration.GetSection("AccountUrl:ConfirmEmail").Value;
                        //    string AcivationLink = url + "?eid=" + newUser.Email + "&tkn=" + token;
                        //    response.Success(_ClientPrimaryInfo);
                        //    //_context.Database.CommitTransaction();
                        //    if (!string.IsNullOrEmpty(_ClientPrimaryInfo.EmailId))
                        //    {
                        //        string emailBody = _IMessageService.GetEmailTemplate();
                        //        string Message = "Welcome to LHS you are successfully registered with us your EmailId is";
                        //        string Subject = "Successfully Registered!";
                        //        string UserName = _ClientPrimaryInfo.FirstName;
                        //        string Password = _ClientPrimaryInfo.FirstName+"@123";
                        //        emailBody = emailBody.Replace("{VerificationLink}", AcivationLink);
                        //        emailBody = emailBody.Replace("{Message}", Message);
                        //        emailBody = emailBody.Replace("{Subject}", Subject);
                        //        emailBody = emailBody.Replace("{UserName}", UserName);
                        //        emailBody = emailBody.Replace("{Email}", _ClientPrimaryInfo.EmailId);
                        //        emailBody = emailBody.Replace("{Password}", Password);
                        //        _IMessageService.SendingEmails(_ClientPrimaryInfo.EmailId, Subject, emailBody);
                        //    }
                        //}
                        //else
                        //{
                        //    _context.Database.RollbackTransaction();
                        //    response.Status = (int)Number.Zero;
                        //    response.Message = ResponseMessage.Error;
                        //    response.ResponseData = null;
                        //}


                    }
                    else
                    {
                        response.AlreadyExist();
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

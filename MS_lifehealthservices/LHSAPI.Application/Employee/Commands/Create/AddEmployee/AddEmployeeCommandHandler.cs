using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
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

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployee
{
    public class AddEmployeeInfoCommandHandler : IRequestHandler<AddEmployeeInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMessageService _IMessageService;

        public AddEmployeeInfoCommandHandler(LHSDbContext context, ISessionService ISessionService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMessageService IMessageService)

        {
            _context = context;
            _ISessionService = ISessionService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _IMessageService = IMessageService;

        }

        public async Task<ApiResponse> Handle(AddEmployeeInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (!string.IsNullOrEmpty(request.Firstname))
                {

                    var ExistUser = _context.EmployeePrimaryInfo.FirstOrDefault(x => x.FirstName == request.Firstname & x.IsActive == true & x.EmailId == request.EmailId);
                    if (ExistUser == null)
                    {
                        EmployeePrimaryInfo user = new EmployeePrimaryInfo();
                        user.FirstName = request.Firstname;
                        user.LastName = request.LastName;
                        user.EmployeeId = Common.CommonMethods.CommonFunction.Get8DigitID();
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.Gender = request.Gender;
                        user.IsActive = true;
                        user.City = request.City;
                        user.State = request.State;
                        user.Country = request.Country;
                        user.DateOfBirth = request.DateOfBirth;
                        user.Saluation = request.Saluation;
                        user.MiddleName = request.MiddleName;
                        user.Status = request.Status;
                        user.Role = request.Role;
                        user.MobileNo = request.MobileNo;
                        user.EmployeeLevel = request.EmployeeLevel;
                        user.EmailId = request.EmailId;
                        user.Code = request.Code;
                        user.Address1 = request.Address1;
                        user.City = request.City;
                        _context.Database.BeginTransaction();
                        await _context.EmployeePrimaryInfo.AddAsync(user);
                        _context.SaveChanges();
                        var newUser = new ApplicationUser
                        {
                            UserName = user.EmailId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            PhoneNumber = user.MobileNo,
                            Email = user.EmailId,
                            PhoneNumberConfirmed = true,
                            EmailConfirmed = false,
                            EmployeeId = user.Id,
                            NormalizedEmail = user.EmailId,
                            NormalizedUserName = user.EmailId
                        };
                        var objNew = await _userManager.CreateAsync(newUser, user.FirstName.ToLower() + "@123");
                        if (objNew.Succeeded)
                        {
                            var currentRole = GetRoles(request.Role).ToString();
                            var isRoleExist = await _roleManager.RoleExistsAsync(currentRole);
                            if (!isRoleExist)
                                await _roleManager.CreateAsync(new IdentityRole(currentRole));

                            var id = newUser.Id;
                            await _userManager.AddClaimAsync(newUser, new Claim("Roles", currentRole));
                            await _userManager.AddToRoleAsync(newUser, currentRole);
                            await _context.SaveChangesAsync();
                            User user1 = new User();
                            user1.UserName = user.EmailId;
                            user1.UserById = newUser.Id;
                            user1.PhoneNo = user.MobileNo;
                            //user.SSN = existrecord.SSN;
                            user1.isLoginFirstTime = true;
                            user1.Lat = 0;
                            user1.Long = 0;

                            await _context.User.AddAsync(user1);
                            _context.SaveChanges();
                            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                            token = System.Web.HttpUtility.UrlEncode(token);
                            string url = _configuration.GetSection("AccountUrl:ConfirmEmail").Value;
                            string AcivationLink = url + "?eid=" + newUser.Email + "&tkn=" + token;
                            response.Success(user);
                            _context.Database.CommitTransaction();
                            var Existpayrate = _context.GlobalPayRate.FirstOrDefault(x => x.Level == request.EmployeeLevel & x.IsActive == true);
                            if (ExistUser == null && request.EmployeeLevel != null)
                            {
                                LHSAPI.Domain.Entities.EmployeePayRate EmployeePayRate = new LHSAPI.Domain.Entities.EmployeePayRate();
                                EmployeePayRate.EmployeeId = user.Id;
                                EmployeePayRate.Holiday12To6AM = Existpayrate.Holiday12To6AM;
                                EmployeePayRate.Holiday6To12AM = Existpayrate.Holiday6To12AM;
                                EmployeePayRate.MonToFri12To6AM = Existpayrate.MonToFri12To6AM;
                                EmployeePayRate.MonToFri6To12AM = Existpayrate.MonToFri6To12AM;
                                EmployeePayRate.Sat12To6AM = Existpayrate.Sat12To6AM;
                                EmployeePayRate.Sat6To12AM = Existpayrate.Sat6To12AM;
                                EmployeePayRate.Sun6To12AM = Existpayrate.Sun6To12AM;
                                EmployeePayRate.Sun12To6AM = Existpayrate.Sun12To6AM;
                                EmployeePayRate.ActiveNightsAndSleep = Existpayrate.ActiveNightsAndSleep;
                                EmployeePayRate.HouseCleaning = Existpayrate.HouseCleaning;
                                EmployeePayRate.TransportPetrol = Existpayrate.TransportPetrol;
                                EmployeePayRate.Level = Existpayrate.Level;
                                EmployeePayRate.IsActive = true;
                                EmployeePayRate.CreatedById = await _ISessionService.GetUserId();
                                EmployeePayRate.CreatedDate = DateTime.Now;
                                await _context.EmployeePayRate.AddAsync(EmployeePayRate);
                                _context.SaveChanges();

                            }
                            if (!string.IsNullOrEmpty(user.EmailId))
                            {
                                string emailBody = _IMessageService.GetEmailTemplate();
                                string Message = "Welcome to LHS you are successfully registered with us your EmailId is";
                                string Subject = "Successfully Registered!";
                                string UserName = user.FirstName;
                                string Password = user.FirstName + "@123";
                                emailBody = emailBody.Replace("{VerificationLink}", AcivationLink);
                                emailBody = emailBody.Replace("{Message}", Message);
                                emailBody = emailBody.Replace("{Subject}", Subject);
                                emailBody = emailBody.Replace("{UserName}", UserName);
                                emailBody = emailBody.Replace("{Email}", user.EmailId);
                                emailBody = emailBody.Replace("{Password}", Password);
                                _IMessageService.SendingEmails(user.EmailId, Subject, emailBody);
                            }
                        }
                        else
                        {
                            _context.Database.RollbackTransaction();
                            response.Status = (int)Number.Zero;
                            //response.Message = ResponseMessage.Error;
                            response.Message = objNew.Errors.FirstOrDefault().Description;
                            response.ResponseData = null;
                        }
                    }
                    else
                    {
                        response.AlreadyExist();

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
        private UserRole GetRoles(int id)
        {
            UserRole role;
            var str = _context.StandardCode.Where(x => x.ID == id).FirstOrDefault().CodeDescription;
            switch (str)
            {
                case "Chief Executive Officer":
                    {
                        role = UserRole.ChiefExecutiveOfficer;
                        break;
                    }
                case "Operations Manager (Proposed)":
                    {
                        role = UserRole.OperationsManager;
                        break;
                    }
                case "General Manager":
                    {
                        role = UserRole.GeneralManager;
                        break;
                    }

                case "Compliance Officer":
                    {
                        role = UserRole.ComplianceOfficer;
                        break;
                    }
                case "Care Coordinator":
                    {
                        role = UserRole.CareCoordinator;
                        break;
                    }
                case "Accounts Officer":
                    {
                        role = UserRole.AccountsOfficer;
                        break;
                    }
                case "Incident Reporting Officer":
                    {
                        role = UserRole.IncidentReportingOfficer;
                        break;
                    }
                case "House Team Leader":
                    {
                        role = UserRole.HouseTeamLeader;
                        break;
                    }
                case "Human Resources Officer":
                    {
                        role = UserRole.HumanResourcesOfficer;
                        break;
                    }
                case "Disability Support Worker":
                    {
                        role = UserRole.DisabilitySupportWorker;
                        break;
                    }
                case "Mental Health Worker/ Disability Support Worker":
                    {
                        role = UserRole.MentalHealthWorkerOrDisabilitySupportWorker;

                        break;
                    }
                default:
                    {
                        role = UserRole.HouseTeamLeader;
                        break;
                    }
            }

            return role;
        }

    }
}


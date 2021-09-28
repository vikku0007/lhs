
using CleanArchitecture.Application.Account.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using LHSAPI.Application.Account.Queries.Login;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;
using LHSAPI.Application.Interface;
using Microsoft.EntityFrameworkCore;
using LHSAPI.Common.Cryptography;

namespace CleanArchitecture.Application.Account.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, object>, IRequestHandler<ForgotPassword, object>, IRequestHandler<ResetPassword, object>, IRequestHandler<EmailConfirmation, object>
    {
        private readonly LHSDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMessageService _IMessageService;
        public LoginQueryHandler()
        {

        }
        public LoginQueryHandler(LHSDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMessageService IMessageService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _IMessageService = IMessageService;
        }

        public async Task<object> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(request.UserName);
                    if (user == null)
                    {
            response.Status = 0;
            response.StatusCode = ResponseCode.BadRequest;
                        response.Message = ResponseMessage.UserNotExist;
                        return response;
                    }
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);


                if (!result.Succeeded)
                {
                    response.Status = 0;
                    response.StatusCode = ResponseCode.Unauthorized;
                    response.Message = ResponseMessage.InvalidPassword;
                    return response;
                }

                var userClaims = await _userManager.GetClaimsAsync(user);

                var roles = _userManager.GetRolesAsync(user);
                userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
                userClaims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email  == null ? user.UserName : user.Email));
                userClaims.Add(new Claim(ClaimTypes.Role, roles.Result[0]));
                userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                string k = _configuration["JwtKey"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(k));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

                var token = new JwtSecurityToken(
                  issuer: _configuration.GetSection("JwtIssuerOptions:Issuer").Value,
                  audience: _configuration.GetSection("JwtIssuerOptions:Audience").Value,
                  claims: userClaims,
                  expires: expires,
                  signingCredentials: creds
                );
                var userdetails = _context.User.FirstOrDefault(x => x.UserById == user.Id);

                UserLogin userloginmodel = new UserLogin();
                userloginmodel.Token = new JwtSecurityTokenHandler().WriteToken(token);
                userloginmodel.IsFirstTimeLogin = userdetails.isLoginFirstTime;
                userloginmodel.UserId = userdetails.UserById;
                if (roles.Result[0] != null && Enum.IsDefined(typeof(UserRole), ((UserRole)Enum.Parse(typeof(UserRole), roles.Result[0]))))
                {
                    userloginmodel.UserRole = (int)((UserRole)Enum.Parse(typeof(UserRole), roles.Result[0]));
                }
                else
                {
                    userloginmodel.UserRole = (int)UserRole.Employee;
                }

                userloginmodel.FName = user.FirstName;
                userloginmodel.LName = user.LastName;

                userdetails.Lat = request.Lat;
                userdetails.Long = request.Lng;
                userdetails.Token = userloginmodel.Token;
                userloginmodel.EmployeeId = user.EmployeeId;
        userloginmodel.ClientId = user.ClientId;
        userloginmodel.IsClient = user.IsClient;


        _context.User.Update(userdetails);
                _context.SaveChanges();


                //var firebasetoken = _context.storeDeviceTokens.FirstOrDefault(x => x.UserById == userdetails.UserById && x.TokenId == request.DiviceTokenId && x.IsDeleted == false);
                //if (firebasetoken == null)
                //{
                //    StoreDeviceToken diviceToken = new StoreDeviceToken
                //    {
                //        TokenId = request.DiviceTokenId,
                //        UserById = userdetails.UserById,
                //        CreatedById = userdetails.UserId,
                //        CreatedDate = DateTime.UtcNow,
                //        IsActive = true,
                //        IsDeleted = false
                //    };
                //    _context.storeDeviceTokens.Add(diviceToken);
                //    _context.SaveChanges();
                //}

                response.StatusCode = ResponseCode.Ok;
                response.Message = ResponseMessage.Success;
                response.ResponseData = userloginmodel;
            }
            catch (Exception ex)
            {
                throw ex;
                //response.StatusCode = ResponseCode.BadRequest;
                //response.Message = ResponseMessage.InvalidPassword;

            }
            //var encryptor = new Encryptor();
            //var encrypted = encryptor.Encrypt(Encoding.Default.GetBytes(JsonConvert.SerializeObject(response)), RNCryptorKey.Secretkey);
            //string encryptcode = Convert.ToBase64String(encrypted);

            //object responseObj = new
            //{
            //    responseData = encryptcode
            //};
            return response;
        }
        public async Task<object> Handle(ResetPassword request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var isExist = await _userManager.FindByEmailAsync(request.Email);
                if (isExist != null)
                {
                    var isUpdated = await _userManager.ResetPasswordAsync(isExist, request.Token, request.NewPassword);
                    if (isUpdated.Succeeded)
                    {
                        #region Sending Alert Mail To User
                        if (!string.IsNullOrEmpty(request.Email))
                        {
                            string emailBody = _IMessageService.GetResetPasswordTemplate();
                            string Subject = "Reset Password";
                            string Message = "Your Password Reset Successfully";
                            emailBody = emailBody.Replace("{Subject}", Subject);
                            emailBody = emailBody.Replace("{Message}", Message);
                            _IMessageService.SendingEmails(request.Email, Subject, emailBody);
                        }
                        else
                        {
                            response.NotFound();
                        }

                        #endregion
                        response.Success(true);
                    }
                    else
                    {
                        response.Failed(isUpdated.Errors.Select(x => x.Description).FirstOrDefault());
                    }
                }
                else
                {
          response.Status = 0;
          response.StatusCode = ResponseCode.BadRequest;
          response.Message = ResponseMessage.UserNotExist;
          return response;
        }
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }

            return response;
        }
        public async Task<object> Handle(ForgotPassword request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();

            if (string.IsNullOrEmpty(request.ForgetPasswordEmailId) && string.IsNullOrWhiteSpace(request.ForgetPasswordEmailId))
            {
                response.ValidationError();
                return response;
            }
            try
            {
                var isExist = await _userManager.FindByEmailAsync(request.ForgetPasswordEmailId);
                if (isExist != null)
                {
                    var currentYear = DateTime.UtcNow.Year.ToString();

                    string token = await _userManager.GeneratePasswordResetTokenAsync(isExist);
                    token = System.Web.HttpUtility.UrlEncode(token);
                    string url = _configuration.GetSection("AccountUrl:ResetPassword").Value;
                    string AcivationLink = url + "?eid=" + request.ForgetPasswordEmailId + "&tkn=" + token;
                    if (!string.IsNullOrEmpty(request.ForgetPasswordEmailId))
                    {
                        string emailBody = _IMessageService.GetForgotPasswordTemplate();
                        string Subject = "Reset Password Link!";
                        emailBody = emailBody.Replace("{Subject}", Subject);
                        emailBody = emailBody.Replace("{VerificationLink}", AcivationLink);
                        emailBody = emailBody.Replace("{Message}", "Please click on Reset Password to Reset your password");
                        _IMessageService.SendingEmails(request.ForgetPasswordEmailId, Subject, emailBody);
                    }
                    response.Success();
                }
                else
                {
          response.Status = 0;
          response.StatusCode = ResponseCode.BadRequest;
          response.Message = ResponseMessage.UserNotExist;
          return response;
        }
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }

            return response;
        }
        public async Task<object> Handle(EmailConfirmation request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrEmpty(request.Token))
            {
                response.ValidationError();
                return response;
            }
            try
            {
                var userDetail = await _userManager.FindByEmailAsync(request.Email);
                //  var token = await _userManager.GenerateEmailConfirmationTokenAsync(userDetail);
                if (userDetail != null)
                {
                    if (!userDetail.EmailConfirmed)
                    {
                        var result = await _userManager.ConfirmEmailAsync(userDetail, request.Token);
                        // If email not confirmed.
                        if (!result.Succeeded)
                        {
                            response.Message = (result.Errors.Select(error => error.Description).Aggregate((allErrors, error) => allErrors += ", " + error));
                            response.StatusCode = HTTPStatusCode.NO_DATA_FOUND;
                        }
                        else
                        {

                            #region code for end user email template

                            if (!string.IsNullOrEmpty(request.Email))
                            {
                                string emailBody = _IMessageService.GetConfirmEmailTemplate();
                                string Subject = "Email Confirmed";
                                string Message = "Your Email has been Confirm Successfully";
                                emailBody = emailBody.Replace("{Subject}", Subject);
                                emailBody = emailBody.Replace("{Message}", Message);
                                _IMessageService.SendingEmails(request.Email, Subject, emailBody);
                            }
                            else
                            {
                                response.NotFound();
                            }
                            #endregion
                            response.Status = 1;
                            response.Success(true);
                        }
                    }
                    else
                    {
                        
                        response.Success(true);
                        response.Status = 2;
                    }
                }
                else
                {
          response.Status = 3;
          response.StatusCode = ResponseCode.BadRequest;
          response.Message = ResponseMessage.UserNotExist;
          return response;
          
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

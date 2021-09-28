

using LHSAPI.Application.Interface;
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

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeMiscInfo
{
    public class AddEmployeePicInfoCommandHandler : IRequestHandler<AddEmployeeMiscInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddEmployeePicInfoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddEmployeeMiscInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {

                    var ExistUser = _context.EmployeeMiscInfo.FirstOrDefault(x => x.EmployeeId == request.EmployeeId & x.IsActive == true);
                    if (ExistUser == null)
                    {
                        LHSAPI.Domain.Entities.EmployeeMiscInfo user = new LHSAPI.Domain.Entities.EmployeeMiscInfo();
                        user.EmployeeId = request.EmployeeId;
                        user.Weight = request.Weight;
                        user.Height = request.Height;
                        user.Smoker = request.Smoker;
                        user.Ethnicity = request.Ethnicity;
                        user.Religion = request.Religion;
                        user.CreatedById = await _ISessionService.GetUserId();
                        user.CreatedDate = DateTime.Now;
                        user.IsActive = true;
                        await _context.EmployeeMiscInfo.AddAsync(user);
                        _context.SaveChanges();
                        LHSAPI.Application.Employee.Models.EmployeeMiscInfo model = new Models.EmployeeMiscInfo();
                         model.EmployeeId = user.EmployeeId;
                        model.Weight = request.Weight;
                        model.Height = request.Height;
                        model.Smoker = request.Smoker;
                        model.Ethnicity = request.Ethnicity;
                        model.Religion = request.Religion;
                        model.ReligionName = _context.StandardCode.Where(x => x.ID == model.Religion).Select(x => x.CodeDescription).FirstOrDefault();
                        model.EthnicityName = _context.StandardCode.Where(x => x.ID == model.Ethnicity).Select(x => x.CodeDescription).FirstOrDefault();
                        response.Success(model);

                    }
                    else
                    {
                        ExistUser.Weight = request.Weight;
                        ExistUser.Height = request.Height;
                        ExistUser.Smoker = request.Smoker;
                        ExistUser.Ethnicity = request.Ethnicity;
                        ExistUser.Religion = request.Religion;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        _context.EmployeeMiscInfo.Update(ExistUser);
                        await _context.SaveChangesAsync();
                        LHSAPI.Application.Employee.Models.EmployeeMiscInfo model = new Models.EmployeeMiscInfo();
                        model.Id = ExistUser.Id;
                        model.EmployeeId = request.EmployeeId;
                        model.Weight = request.Weight;
                        model.Height = request.Height;
                        model.Smoker = request.Smoker;
                        model.Ethnicity = request.Ethnicity;
                        model.Religion = request.Religion;
                        model.ReligionName = _context.StandardCode.Where(x => x.ID == model.Religion).Select(x => x.CodeDescription).FirstOrDefault();
                        model.EthnicityName = _context.StandardCode.Where(x => x.ID == model.Ethnicity).Select(x => x.CodeDescription).FirstOrDefault();
                        response.Update(model);

                    }
                }
                else
                {
                    response.ValidationError();
                    //var existrecord = _context.UserRegister.FirstOrDefault(x => x.Id == request.Id);
                    //if (existrecord != null)
                    //{
                    //  existrecord.FirstName = request.Firstname;
                    //  existrecord.LastName = request.LastName;
                    //  existrecord.MiddleName = request.MiddleName;
                    //  //existrecord.EmailId = request.EmailId;
                    //  //   existrecord.Password = request.Password;
                    //  existrecord.UpdatedDate = DateTime.UtcNow;
                    //  //existrecord.PhoneNo = request.Pho;
                    //  //existrecord.OTP = number;
                    //  //existrecord.OTPStartDateTime = DateTime.UtcNow;
                    //  //existrecord.OTPEndDateTime = DateTime.UtcNow.AddMinutes(5);

                    //  _context.Update(existrecord);
                    //  await _context.SaveChangesAsync();
                    //  //SendOTPMessage.SendMessage(request.PhoneNo, number);

                    //  response.Status = (int)Number.One;
                    //  response.Message = ResponseMessage.Success;
                    //  response.ResponseData = existrecord;
                    //}
                    //else
                    //{
                    //  response.Status = (int)Number.Zero;
                    //  response.Message = ResponseMessage.PhoneExist;
                    //}

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

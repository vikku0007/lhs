
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

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeAwardInfo
{
    public class AddEmployeeAwardInfoCommandHandler : IRequestHandler<AddEmployeeAwardInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddEmployeeAwardInfoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddEmployeeAwardInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {

                    var ExistUser = _context.EmployeeAwardInfo.FirstOrDefault(x => x.EmployeeId == request.EmployeeId & x.IsActive == true);
                    if (ExistUser == null)
                    {
                        LHSAPI.Domain.Entities.EmployeeAwardInfo EmployeeKinInfo = new LHSAPI.Domain.Entities.EmployeeAwardInfo();
                        EmployeeKinInfo.EmployeeId = request.EmployeeId;
                        EmployeeKinInfo.Allowances = request.Allowances;
                        EmployeeKinInfo.Dailyhours = request.Dailyhours;
                        EmployeeKinInfo.Weeklyhours = request.Weeklyhours;
                        EmployeeKinInfo.AwardGroup = request.AwardGroup;
                        EmployeeKinInfo.CreatedById = await _ISessionService.GetUserId();
                        EmployeeKinInfo.CreatedDate = DateTime.Now;
                        EmployeeKinInfo.IsActive = true;
                        await _context.EmployeeAwardInfo.AddAsync(EmployeeKinInfo);
                        _context.SaveChanges();
                        response.Success(EmployeeKinInfo);

                    }
                    else
                    {

                        ExistUser.EmployeeId = request.EmployeeId;
                        ExistUser.Allowances = request.Allowances;
                        ExistUser.Dailyhours = request.Dailyhours;
                        ExistUser.Weeklyhours = request.Weeklyhours;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        _context.EmployeeAwardInfo.Update(ExistUser);
                        _context.SaveChanges();
                        response.Update(ExistUser);

                    }
                }
                else
                {
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

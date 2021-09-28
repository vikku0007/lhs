
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

namespace LHSAPI.Application.Employee.Commands.Create.AddPayRate
{
    public class AddPayRateCommandHandler : IRequestHandler<AddPayRateCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddPayRateCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddPayRateCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {

                    var ExistUser = _context.EmployeePayRate.FirstOrDefault(x => x.EmployeeId == request.EmployeeId & x.IsActive == true);
                    if (ExistUser == null)
                    {
                        LHSAPI.Domain.Entities.EmployeePayRate EmployeePayRate = new LHSAPI.Domain.Entities.EmployeePayRate();
                        EmployeePayRate.EmployeeId = request.EmployeeId;
                        EmployeePayRate.Holiday12To6AM = request.Holiday12To6AM;
                        EmployeePayRate.Holiday6To12AM = request.Holiday6To12AM;
                        EmployeePayRate.MonToFri12To6AM = request.MonToFri12To6AM;
                        EmployeePayRate.MonToFri6To12AM = request.MonToFri6To12AM;
                        EmployeePayRate.Sat12To6AM = request.Sat12To6AM;
                        EmployeePayRate.Sat6To12AM = request.Sat6To12AM;
                        EmployeePayRate.Sun6To12AM = request.Sun6To12AM;
                        EmployeePayRate.Sun12To6AM = request.Sun12To6AM;
                        EmployeePayRate.ActiveNightsAndSleep = request.ActiveNightsAndSleep;
                        EmployeePayRate.HouseCleaning = request.HouseCleaning;
                        EmployeePayRate.TransportPetrol = request.TransportPetrol;
                        EmployeePayRate.Level = request.Level;
                        EmployeePayRate.IsActive = true;
                        EmployeePayRate.CreatedById = await _ISessionService.GetUserId();
                        EmployeePayRate.CreatedDate = DateTime.Now;
                        await _context.EmployeePayRate.AddAsync(EmployeePayRate);
                        _context.SaveChanges();
                        response.Success(EmployeePayRate);
                    }
                    else
                    {
                        ExistUser.Holiday12To6AM = request.Holiday12To6AM;
                        ExistUser.Holiday6To12AM = request.Holiday6To12AM;
                        ExistUser.MonToFri12To6AM = request.MonToFri12To6AM;
                        ExistUser.MonToFri6To12AM = request.MonToFri6To12AM;
                        ExistUser.Sat12To6AM = request.Sat12To6AM;
                        ExistUser.Sat6To12AM = request.Sat6To12AM;
                        ExistUser.Sun6To12AM = request.Sun6To12AM;
                        ExistUser.Sun12To6AM = request.Sun12To6AM;
                        ExistUser.ActiveNightsAndSleep = request.ActiveNightsAndSleep;
                        ExistUser.HouseCleaning = request.HouseCleaning;
                        ExistUser.TransportPetrol = request.TransportPetrol;
                        ExistUser.Level = request.Level;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        _context.EmployeePayRate.Update(ExistUser);
                        await _context.SaveChangesAsync();
                        response.Update(ExistUser);

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

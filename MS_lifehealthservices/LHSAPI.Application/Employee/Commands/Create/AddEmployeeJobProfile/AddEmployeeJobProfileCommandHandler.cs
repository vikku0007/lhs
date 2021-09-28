
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

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeJobProfile
{
    public class AddEmployeeJobProfileCommandHandler : IRequestHandler<AddEmployeeJobProfileCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddEmployeeJobProfileCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddEmployeeJobProfileCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {

                    var ExistUser = _context.EmployeeJobProfile.FirstOrDefault(x => x.EmployeeId == request.EmployeeId & x.IsActive == true &&
                    x.Id == request.Id && x.Id != 0);
                    if (ExistUser == null)
                    {
                        LHSAPI.Domain.Entities.EmployeeJobProfile EmployeeJobProfile = new LHSAPI.Domain.Entities.EmployeeJobProfile();
                        EmployeeJobProfile.EmployeeId = request.EmployeeId;
                        EmployeeJobProfile.DateOfJoining = request.DateOfJoining;
                        EmployeeJobProfile.DepartmentId = request.DepartmentId;
                        EmployeeJobProfile.DistanceTravel = request.DistanceTravel;
                        EmployeeJobProfile.JobDesc = request.JobDesc;
                      //  EmployeeJobProfile.LocationId = request.LocationId;
                        EmployeeJobProfile.Source = request.Source;
                        EmployeeJobProfile.ReportingToId = request.ReportingToId;
                      //  EmployeeJobProfile.LocationType = request.LocationType;
                      //  EmployeeJobProfile.OtherLocation = request.OtherLocation;
                        EmployeeJobProfile.CreatedById = await _ISessionService.GetUserId();
                        EmployeeJobProfile.CreatedDate = DateTime.Now;
                        EmployeeJobProfile.WorkingHoursWeekly = request.WorkingHoursWeekly;
                        EmployeeJobProfile.IsActive = true;
                        await _context.EmployeeJobProfile.AddAsync(EmployeeJobProfile);
                        _context.SaveChanges();
                        LHSAPI.Application.Employee.Models.EmployeeJobProfile model = new Models.EmployeeJobProfile();
                        model.EmployeeId = request.EmployeeId;
                        model.Id = EmployeeJobProfile.Id;
                        model.DateOfJoining = request.DateOfJoining;
                        model.DepartmentId = request.DepartmentId;
                        model.DistanceTravel = request.DistanceTravel;
                        model.JobDesc = request.JobDesc;
                        model.Source = request.Source;
                        model.ReportingToId = request.ReportingToId;
                        model.WorkingHoursWeekly = request.WorkingHoursWeekly;
                        model.LocationTypeName = _context.StandardCode.Where(x => x.Value == model.LocationType).Select(x => x.CodeDescription).FirstOrDefault();
                        model.LocationName = model.OtherLocation != "" ? model.OtherLocation : _context.Location.Where(x => x.LocationId == model.LocationId).Select(x => x.Name).FirstOrDefault();
                      
                        response.Success(model);
                    }
                    else
                    {

                        ExistUser.DateOfJoining = request.DateOfJoining;
                        ExistUser.DepartmentId = request.DepartmentId;
                        ExistUser.DistanceTravel = request.DistanceTravel;
                        ExistUser.JobDesc = request.JobDesc;
                        ExistUser.Source = request.Source;
                        ExistUser.ReportingToId = request.ReportingToId;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        ExistUser.WorkingHoursWeekly = request.WorkingHoursWeekly;
                        _context.EmployeeJobProfile.Update(ExistUser);
                        await _context.SaveChangesAsync();
                        LHSAPI.Application.Employee.Models.EmployeeJobProfile model = new Models.EmployeeJobProfile();
                        model.EmployeeId = request.EmployeeId;
                        model.Id = ExistUser.Id;
                        model.DateOfJoining = request.DateOfJoining;
                        model.DepartmentId = request.DepartmentId;
                        model.DistanceTravel = request.DistanceTravel;
                        model.JobDesc = request.JobDesc;
                        model.Source = request.Source;
                        model.ReportingToId = request.ReportingToId;
                        model.WorkingHoursWeekly = request.WorkingHoursWeekly;
                        model.LocationTypeName = _context.StandardCode.Where(x => x.Value == model.LocationType).Select(x => x.CodeDescription).FirstOrDefault();
                        model.LocationName = model.OtherLocation != "" ? model.OtherLocation : _context.Location.Where(x => x.LocationId == model.LocationId).Select(x => x.Name).FirstOrDefault();
                       
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
                    // 
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

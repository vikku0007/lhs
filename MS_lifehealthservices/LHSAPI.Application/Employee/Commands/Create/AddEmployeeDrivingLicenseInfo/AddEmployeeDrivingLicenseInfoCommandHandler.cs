
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

namespace LHSAPI.Application.Employee.Commands.Create.AddEmployeeDrivingLicenseInfo
{
    public class AddEmployeeDrivingLicenseInfoCommandHandler : IRequestHandler<AddEmployeeDrivingLicenseInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddEmployeeDrivingLicenseInfoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddEmployeeDrivingLicenseInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.EmployeeId > 0)
                {

                    var ExistUser = _context.EmployeeDrivingLicenseInfo.FirstOrDefault(x => x.EmployeeId == request.EmployeeId & x.IsActive == true &&
                    x.Id == request.Id && request.Id != 0);
                    if (ExistUser == null)
                    {
                        LHSAPI.Domain.Entities.EmployeeDrivingLicenseInfo EmployeeDrivingLicenseInfo = new LHSAPI.Domain.Entities.EmployeeDrivingLicenseInfo();
                        EmployeeDrivingLicenseInfo.EmployeeId = request.EmployeeId;
                        EmployeeDrivingLicenseInfo.DrivingLicense = request.DrivingLicense;
                        EmployeeDrivingLicenseInfo.CarInsurance = request.CarInsurance;
                        EmployeeDrivingLicenseInfo.CarRegExpiryDate = request.CarRegExpiryDate;
                        EmployeeDrivingLicenseInfo.CarRegNo = request.CarRegNo;
                        EmployeeDrivingLicenseInfo.LicenseExpiryDate = request.LicenseExpiryDate;
                        EmployeeDrivingLicenseInfo.InsuranceExpiryDate = request.InsuranceExpiryDate;
                        EmployeeDrivingLicenseInfo.LicenseNo = request.LicenseNo;
                        EmployeeDrivingLicenseInfo.LicenseState = request.LicenseState;
                        EmployeeDrivingLicenseInfo.LicenseType = request.LicenseType;
                        EmployeeDrivingLicenseInfo.LicenseOrigin = request.LicenseOrigin;
                        EmployeeDrivingLicenseInfo.CreatedById = await _ISessionService.GetUserId();
                        EmployeeDrivingLicenseInfo.CreatedDate = DateTime.Now;
                        EmployeeDrivingLicenseInfo.IsActive = true;
                        await _context.EmployeeDrivingLicenseInfo.AddAsync(EmployeeDrivingLicenseInfo);
                        _context.SaveChanges();
                        LHSAPI.Application.Employee.Models.EmployeeDriverLicenseModel model = new Models.EmployeeDriverLicenseModel();
                        model.EmployeeId = request.EmployeeId;
                        model.DrivingLicense = request.DrivingLicense;
                        model.CarInsurance = request.CarInsurance;
                        model.CarRegExpiryDate = request.CarRegExpiryDate;
                        model.CarRegNo = request.CarRegNo;
                        model.LicenseExpiryDate = request.LicenseExpiryDate;
                        model.InsuranceExpiryDate = request.InsuranceExpiryDate;
                        model.LicenseNo = request.LicenseNo;
                        model.LicenseState = request.LicenseState;
                        model.LicenseType = request.LicenseType;
                        model.LicenseTypeName = _context.StandardCode.Where(x => x.ID == model.LicenseType).Select(x => x.CodeDescription).FirstOrDefault();
                        model.LicenseStateName = _context.StandardCode.Where(x => x.ID == model.LicenseState).Select(x => x.CodeDescription).FirstOrDefault();
                        response.Success(model);
                       

                    }
                    else
                    {
                        // LHSAPI.Domain.Entities.EmployeeDrivingLicenseInfo EmployeeDrivingLicenseInfo = new LHSAPI.Domain.Entities.EmployeeDrivingLicenseInfo();
                        // EmployeeDrivingLicenseInfo.EmployeeId = request.EmployeeId;
                        ExistUser.DrivingLicense = request.DrivingLicense;
                        ExistUser.CarInsurance = request.CarInsurance;
                        ExistUser.CarRegExpiryDate = request.CarRegExpiryDate;
                        ExistUser.CarRegNo = request.CarRegNo;
                        ExistUser.LicenseExpiryDate = request.LicenseExpiryDate;
                        ExistUser.InsuranceExpiryDate = request.InsuranceExpiryDate;
                        ExistUser.LicenseNo = request.LicenseNo;
                        ExistUser.LicenseState = request.LicenseState;
                        ExistUser.LicenseType = request.LicenseType;
                        ExistUser.LicenseOrigin = request.LicenseOrigin;
                        ExistUser.UpdateById = 1;
                        ExistUser.CreatedDate = DateTime.Now;
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        ExistUser.IsActive = true;
                        _context.EmployeeDrivingLicenseInfo.Update(ExistUser);
                        _context.SaveChanges();
                        LHSAPI.Application.Employee.Models.EmployeeDriverLicenseModel model = new Models.EmployeeDriverLicenseModel();
                        model.EmployeeId = request.EmployeeId;
                        model.DrivingLicense = request.DrivingLicense;
                        model.CarInsurance = request.CarInsurance;
                        model.CarRegExpiryDate = request.CarRegExpiryDate;
                        model.CarRegNo = request.CarRegNo;
                        model.LicenseExpiryDate = request.LicenseExpiryDate;
                        model.InsuranceExpiryDate = request.InsuranceExpiryDate;
                        model.LicenseNo = request.LicenseNo;
                        model.LicenseState = request.LicenseState;
                        model.LicenseType = request.LicenseType;
                        model.LicenseTypeName = _context.StandardCode.Where(x => x.ID == model.LicenseType).Select(x => x.CodeDescription).FirstOrDefault();
                        model.LicenseStateName = _context.StandardCode.Where(x => x.ID == model.LicenseState).Select(x => x.CodeDescription).FirstOrDefault();
                        response.Update(model);

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

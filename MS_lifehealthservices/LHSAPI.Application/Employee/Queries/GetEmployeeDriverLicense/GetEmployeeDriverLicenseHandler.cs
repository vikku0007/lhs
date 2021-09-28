
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using static LHSAPI.Common.Enums.ResponseEnums;
using LHSAPI.Domain.Entities;

namespace LHSAPI.Application.Employee.Queries.GetEmployeeDriverLicense
{
    public class GetEmployeeDriverLicenseHandler : IRequestHandler<GetEmployeeDriverLicenseListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeDriverLicenseHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
       
        public async Task<ApiResponse> Handle(GetEmployeeDriverLicenseListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                var AvbempList = (from emp in _dbContext.EmployeeDrivingLicenseInfo
                                                               where emp.IsActive == true && emp.IsDeleted == false && emp.EmployeeId == request.EmployeeId
                                                               select new LHSAPI.Application.Employee.Models.EmployeeDriverLicenseModel
                                                               {
                                                                   Id = emp.Id,
                                                                   EmployeeId = emp.EmployeeId,
                                                                   DrivingLicense = emp.DrivingLicense,
                                                                   CarInsurance = emp.CarInsurance,
                                                                   CarRegExpiryDate = emp.CarRegExpiryDate,
                                                                   CarRegNo = emp.CarRegNo,
                                                                   LicenseType = emp.LicenseType,
                                                                   LicenseState = emp.LicenseState,
                                                                   LicenseNo = emp.LicenseNo,
                                                                   LicenseOrigin = emp.LicenseOrigin,
                                                                   LicenseExpiryDate = emp.LicenseExpiryDate,
                                                                   InsuranceExpiryDate = emp.InsuranceExpiryDate,
                                                                   LicenseTypeName = _dbContext.StandardCode.Where(x => x.ID == emp.LicenseType).Select(x => x.CodeDescription).FirstOrDefault(),
                                                                   LicenseStateName = _dbContext.StandardCode.Where(x => x.ID == emp.LicenseState).Select(x => x.CodeDescription).FirstOrDefault(),
                                                                   LicenseOriginName = _dbContext.StandardCode.Where(x => x.ID == emp.LicenseOrigin).Select(x => x.CodeDescription).FirstOrDefault()
                                                               }).OrderByDescending(x=>x.Id).ToList();
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(AvbempList);

                }
                else
                {
                    response = response.NotFound();
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

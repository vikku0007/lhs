
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
using LHSAPI.Application.Employee.Models;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Employee.Queries.GetEmployeePrimaryInfo
{
    public class EmployeePrimaryInfoHandler : IRequestHandler<GetEmployeePrimaryInfo, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public EmployeePrimaryInfoHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
        #region Employee
        /// <summary>
        /// Get Particular Employee
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Handle(GetEmployeePrimaryInfo request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                var empList = _dbContext.EmployeePrimaryInfo.Where(x => x.IsDeleted == false && x.IsActive && x.Id == request.Id).FirstOrDefault();
                if (empList != null && empList.Id > 0)
                {
                    EmployeePrimaryInfoViewModel model = new EmployeePrimaryInfoViewModel();
                    model.Id = empList.Id;

                    model.Saluation = empList.Saluation;

                    model.FirstName = empList.FirstName;

                    model.MiddleName = empList.MiddleName;

                    model.LastName = empList.LastName;

                    model.Role = empList.Role;

                    model.DateOfBirth = empList.DateOfBirth;

                    model.MaritalStatus = empList.MaritalStatus;

                    model.MobileNo = empList.MobileNo;

                    model.Gender = empList.Gender;

                    model.EmailId = empList.EmailId;

                    model.EmployeeId = empList.EmployeeId;

                    model.EmployeeLevel = empList.EmployeeLevel;

                    model.Status = empList.Status;
                    model.Address1 = empList.Address1;

                    model.City = empList.City;

                    model.State = empList.State;

                    model.Country = empList.Country;

                    model.Code = empList.Code;
                    model.EmpType = empList.EmpType;
                    model.Language = empList.Language;
                    model.RoleName = _dbContext.StandardCode.Where(x => x.ID == model.Role).Select(x => x.CodeDescription).FirstOrDefault();
                    model.SalutationName = _dbContext.StandardCode.Where(x => x.ID == model.Saluation).Select(x => x.CodeDescription).FirstOrDefault();
                    model.HasVisa = empList.HasVisa;
                    model.VisaNumber = empList.VisaNumber;
                    model.PassportNumber = empList.PassportNumber;
                    model.VisaType = empList.VisaType;
                    model.VisaExpiryDate = empList.VisaExpiryDate;
                    model.VisaTypeName = _dbContext.StandardCode.Where(x => x.ID == model.VisaType).Select(x => x.CodeDescription).FirstOrDefault();
                    model.CountryName = empList.Country;
                    model.StateName = empList.State;
                    model.Religion = empList.Religion;
                    response.SuccessWithOutMessage(model);
                }
                else
                {
                    response.NotFound();
                }

            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }
        #endregion
    }
}

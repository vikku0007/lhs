
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
using System.Security.Cryptography;
using LHSAPI.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace LHSAPI.Application.Employee.Queries.GetEmployeePasswordInfo
{
    public class GetEmployeePasswordHandler : IRequestHandler<GetEmployeePasswordInfo, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;

        //   readonly ILoggerManager _logger;
        public GetEmployeePasswordHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetEmployeePasswordInfo request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                EmployeeDetails _EmployeeDetails = new EmployeeDetails();
               
              
                var empList = (from emp in _dbContext.EmployeePrimaryInfo
                               where emp.IsActive == true && emp.IsDeleted == false && emp.Id == request.Id
                               select new EmployeePrimaryInfoViewModel
                               {
                                   Id = emp.Id,
                                 
                                   EmailId = emp.EmailId,
                                   EmployeeId = emp.EmployeeId,
                                   PasswordExist = request.Id
                                 
                               }).ToList();
             

                if (empList != null)
                {
                  
                    response.SuccessWithOutMessage(empList);
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

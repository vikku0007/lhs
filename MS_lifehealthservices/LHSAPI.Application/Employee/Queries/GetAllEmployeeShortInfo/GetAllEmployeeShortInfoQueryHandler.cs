
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
using LHSAPI.Common.Enums;

namespace LHSAPI.Application.Employee.Queries.GetAllEmployeeShortInfo
{
    public class GetAllEmployeeShortInfoQueryHandler : IRequestHandler<GetAllEmployeeShortInfoQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetAllEmployeeShortInfoQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
        #region My Leagues
        /// <summary>
        /// Get List Of All Leagues Of Particular User
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Handle(GetAllEmployeeShortInfoQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                //var empList = _dbContext.EmployeePrimaryInfo.Where(x => x.IsDeleted == false && x.IsActive).OrderByDescending(x => x.Id).ToList();
                var empList = (from emp in _dbContext.EmployeePrimaryInfo
                               where emp.IsActive == true && emp.IsDeleted == false
                               select new
                               {
                                   Id = emp.Id,
                                   FullName = emp.FirstName + " " + ((emp.MiddleName == null) ? "" : " " + emp.MiddleName) + " " + ((emp.LastName == null) ? "" : " " + emp.LastName),
                               }).OrderByDescending(x => x.Id).ToList();
               
                if (empList != null && empList.Any())
                { 
                    response.SuccessWithOutMessage(empList.ToList());
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











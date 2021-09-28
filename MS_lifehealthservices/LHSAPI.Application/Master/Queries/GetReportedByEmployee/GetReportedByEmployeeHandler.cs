
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

namespace LHSAPI.Application.Master.Queries.GetReportedByEmployee
{
    public class GetReportedByEmployeeQueryHandler : IRequestHandler<GetReportedByEmployeeQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetReportedByEmployeeQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetReportedByEmployeeQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                //var empList = _dbContext.EmployeePrimaryInfo.Where(x => x.IsDeleted == false && x.IsActive).OrderByDescending(x => x.Id).ToList();
                var empList = (from emp in _dbContext.EmployeeJobProfile
                               where emp.IsActive == true && emp.IsDeleted == false && emp.EmployeeId == request.EmployeeId
                               select new
                               {
                                   Id = emp.ReportingToId,
                                   FullName = _dbContext.EmployeePrimaryInfo.Where(x => x.Id == emp.ReportingToId).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                                   PhoneNo= _dbContext.EmployeePrimaryInfo.Where(x => x.Id == emp.ReportingToId).Select(x => x.MobileNo).FirstOrDefault()

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











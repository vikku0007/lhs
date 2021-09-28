
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

namespace LHSAPI.Application.Master.Queries.GetImageDetail
{
    public class GetImageDetailHandler : IRequestHandler<GetImageDetailQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetImageDetailHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetImageDetailQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                    var imageUrl = _dbContext.EmployeePicInfo.Where(x => x.EmployeeId == request.Id).Select(x => x.Path).Any() ? _dbContext.EmployeePicInfo.Where(x => x.EmployeeId == request.Id).Select(x => x.Path).FirstOrDefault() : null;
                    var empList = (from emp in _dbContext.EmployeePrimaryInfo
                                   where emp.IsActive == true && emp.IsDeleted == false && emp.Id == request.Id
                                   select new
                                   {
                                       Id = emp.Id,
                                       FullName = emp.FirstName + " " + ((emp.MiddleName == null) ? "" : " " + emp.MiddleName) + " " + ((emp.LastName == null) ? "" : " " + emp.LastName),
                                       ImageUrl=imageUrl
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

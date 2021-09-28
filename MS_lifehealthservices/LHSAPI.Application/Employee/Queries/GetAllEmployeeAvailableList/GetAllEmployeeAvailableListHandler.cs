
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

namespace LHSAPI.Application.Employee.Queries.GetAllEmployeeAvailableList
{
    public class GetAllEmployeeAvailableListQueryHandler : IRequestHandler<GetAllEmployeeAvailableListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetAllEmployeeAvailableListQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetAllEmployeeAvailableListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var AvbempList = (from Employeedata in _dbContext.EmployeePrimaryInfo
                                  join RequireComp in _dbContext.EmployeeAvailabilityDetails on Employeedata.Id equals RequireComp.EmployeeId
                                  where RequireComp.IsDeleted == false && RequireComp.IsActive == true 
                                  select new
                                  {
                                      RequireComp,
                                      Employeedata.FirstName,
                                      Employeedata.MiddleName,
                                      Employeedata.LastName,

                                  }).ToList();
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();
                    AvbempList = AvbempList.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(AvbempList.ToList());
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

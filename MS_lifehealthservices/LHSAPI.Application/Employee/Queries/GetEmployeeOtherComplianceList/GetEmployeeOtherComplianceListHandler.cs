
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

namespace LHSAPI.Application.Employee.Queries.GetEmployeeOtherComplianceList
{
    public class GetEmployeeOtherComplianceListHandler : IRequestHandler<GetEmployeeOtherComplianceListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeOtherComplianceListHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetEmployeeOtherComplianceListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var commList = (from Employeedata in _dbContext.EmployeePrimaryInfo
                                  join RequireComp in _dbContext.EmployeeOtherComplianceDetails on Employeedata.Id equals RequireComp.EmployeeId
                                  where RequireComp.IsDeleted == false && RequireComp.IsActive == true && RequireComp.EmployeeId==request.EmployeeId
                                select new
                                  {
                                      RequireComp.Id,
                                      RequireComp.OtherAlert,
                                      RequireComp.OtherDocumentName,
                                    //  RequireComp.OtherDocumentType,
                                      RequireComp.OtherDescription,
                                      RequireComp.OtherIssueDate,
                                      RequireComp.OtherExpiryDate,
                                      RequireComp.OtherHasExpiry,
                                      Employeedata.FirstName,
                                      Employeedata.MiddleName,
                                      Employeedata.LastName,
                                      FullName = Employeedata.FirstName + " " + Employeedata.MiddleName + " " + Employeedata.LastName,
                                   //  OtherDocumentypeName = _dbContext.StandardCode.Where(x => x.ID == RequireComp.OtherDocumentType).Select(x => x.CodeDescription).FirstOrDefault(),
                                    OtherDocument = _dbContext.StandardCode.Where(x => x.ID == RequireComp.OtherDocumentName).Select(x => x.CodeDescription).FirstOrDefault(),
                                    OtherFileName = RequireComp.OtherFileName,
                                    CreatedDate = RequireComp.CreatedDate
                                }).OrderByDescending(x => x.Id).ToList();
                //var commList = _dbContext.EmployeeOtherComplianceDetails.Where(x => x.EmployeeId == request.EmployeeId && x.IsActive == true
                //   && x.IsDeleted == false).AsQueryable().ToList();
                if (commList != null && commList.Count > 0)
                {
                    commList = commList.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                    var totalCount = commList.Count;
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(commList.ToList());
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

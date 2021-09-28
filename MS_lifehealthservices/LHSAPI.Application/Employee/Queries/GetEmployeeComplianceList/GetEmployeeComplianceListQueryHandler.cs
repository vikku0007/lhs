
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

namespace LHSAPI.Application.Employee.Queries.GetEmployeeComplianceList
{
    public class GetEmployeeComplianceListQueryHandler : IRequestHandler<GetEmployeeComplianceListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeComplianceListQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetEmployeeComplianceListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                //var commList = _dbContext.EmployeeCompliancesDetails.Where(x => x.EmployeeId == request.EmployeeId && x.IsActive == true
                //  && x.IsDeleted == false).AsQueryable().OrderByDescending(x => x.Id).ToList();
                var commList = (from RequireComp in _dbContext.EmployeeCompliancesDetails where RequireComp.IsDeleted == false && RequireComp.IsActive == true && RequireComp.EmployeeId == request.EmployeeId
                                select new
                                {
                                    Id = RequireComp.Id,
                                    Alert = RequireComp.Alert,
                                    DocumentName = RequireComp.DocumentName,
                                    DocumentType = RequireComp.TrainingType,
                                    DocumentTypeName = _dbContext.StandardCode.Where(x => x.ID == RequireComp.TrainingType).Select(x => x.CodeDescription).FirstOrDefault(),
                                    Description = RequireComp.Description,
                                    IssueDate = RequireComp.IssueDate,
                                    HasExpiry = RequireComp.HasExpiry,
                                    ExpiryDate = RequireComp.ExpiryDate,
                                    EmployeeId = RequireComp.EmployeeId,
                                    Document = _dbContext.StandardCode.Where(x => x.ID == RequireComp.DocumentName).Select(x => x.CodeDescription).FirstOrDefault(),
                                    FileName = RequireComp.FileName,
                                    CreatedDate = RequireComp.CreatedDate
                                }).OrderByDescending(x => x.Id).ToList();
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

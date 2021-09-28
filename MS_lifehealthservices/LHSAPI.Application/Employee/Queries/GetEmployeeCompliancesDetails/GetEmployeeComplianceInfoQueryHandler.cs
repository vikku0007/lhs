
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

namespace LHSAPI.Application.Employee.Queries.GetEmployeeCompliancesDetails
{
    public class GetEmployeeCompliancesDetailsQueryHandler : IRequestHandler<GetEmployeeCompliancesInfoQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeCompliancesDetailsQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }
       
        public async Task<ApiResponse> Handle(GetEmployeeCompliancesInfoQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                var CompList = _dbContext.EmployeeCompliancesDetails.Where( x => x.Id == request.Id && x.IsDeleted == false && x.IsActive
                ).ToList();
                if (CompList != null && CompList.Any())
                {
                    var totalCount = CompList.Count();
                    response.ResponseData = CompList.ToList();
                    response.Message = ResponseMessage.Success;
                    response.StatusCode = HTTPStatusCode.SUCCESSSTATUSCODE;
                    response.Total = totalCount;
                }
                else
                {
                    response.Message = ResponseMessage.NOTFOUND;
                    response.StatusCode = HTTPStatusCode.NO_DATA_FOUND;
                }

            }
            catch (Exception ex)
            {
        response.Status = (int)Number.Zero;
        response.Message = ResponseMessage.Error;
                response.StatusCode = HTTPStatusCode.INTERNAL_SERVER_ERROR;
            }
            return response;
        }
       
    }
}

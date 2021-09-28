
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
using LHSAPI.Application.Employee.Models;

namespace LHSAPI.Application.Client.Queries.GetClientMedicalInfo
{
    public class GetClientMedicalInfoHandler : IRequestHandler<GetClientMedicalInfoQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetClientMedicalInfoHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
       
        public async Task<ApiResponse> Handle(GetClientMedicalInfoQuery request, CancellationToken cancellationToken)
        {
            
            ApiResponse response = new ApiResponse();
            try
            {
                var CompList = _dbContext.ClientMedicalHistory.Where(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive
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
                response.Failed(ex.Message);
            }
            return response;
        }
       
    }
}

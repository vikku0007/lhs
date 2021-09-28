
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

namespace LHSAPI.Application.Employee.Queries.GetEmployeeAvailableList
{
    public class GetEmployeeAvailableListQueryHandler : IRequestHandler<GetEmployeeAvailableListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeAvailableListQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
       
        public async Task<ApiResponse> Handle(GetEmployeeAvailableListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                var commList = _dbContext.EmployeeAvailabilityDetails.Where(x => x.EmployeeId == request.EmployeeId && x.IsActive == true
                  && x.IsDeleted == false).AsQueryable().ToList();
                if (commList != null && commList.Count > 0)
                {
                    commList = commList.Skip<EmployeeAvailabilityDetails>((request.PageNo - 1) * request.PageSize).Take<EmployeeAvailabilityDetails>(request.PageSize).ToList();
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
      
    }
}

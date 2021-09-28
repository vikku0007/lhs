
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

namespace LHSAPI.Application.Administration.Queries.GetLocationDetails
{
    public class GetLocationDetailQueryHandler : IRequestHandler<GetLocationDetailQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetLocationDetailQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }
       
        public async Task<ApiResponse> Handle(GetLocationDetailQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                var LocationList = _dbContext.Location.Where(x => x.IsDeleted == false && x.IsActive && x.LocationId == request.LocationId
                );
                if (LocationList != null && LocationList.Any())
                {
                    var totalCount = LocationList.Count();
                    //LocationList = LocationList.Skip<Location>((request.PageNo - 1) * request.PageSize).Take<Location>(request.PageSize);
                    response.ResponseData = LocationList.ToList();
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

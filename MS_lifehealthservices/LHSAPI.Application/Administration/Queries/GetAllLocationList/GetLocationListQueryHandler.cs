
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

namespace LHSAPI.Application.Administration.Queries.GetAllLocationList
{
    public class GetLocationListQueryHandler : IRequestHandler<GetLocationListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetLocationListQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<ApiResponse> Handle(GetLocationListQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var LocationList = _dbContext.Location.Where(x => x.IsDeleted == false && x.IsActive
                && ((string.IsNullOrEmpty(request.SearchTextByName) || (x.Name.Contains(request.SearchTextByName))
                || (x.Address.Contains(request.SearchTextByName)))));
                if (LocationList != null && LocationList.Any())
                {
                    var totalCount = LocationList.Count();
                    switch (request.OrderBy)
                    {
                        case Common.Enums.LocationOrderBy.Name:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                LocationList = LocationList.OrderBy(x => x.Name);
                            }
                            else
                            {
                                LocationList = LocationList.OrderByDescending(x => x.Name);
                            }
                            break;
                        case Common.Enums.LocationOrderBy.Address:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                LocationList = LocationList.OrderBy(x => x.Address);
                            }
                            else
                            {
                                LocationList = LocationList.OrderByDescending(x => x.Address);
                            }
                            break;
                        case Common.Enums.LocationOrderBy.Status:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                LocationList = LocationList.OrderBy(x => x.Status);
                            }
                            else
                            {
                                LocationList = LocationList.OrderByDescending(x => x.Status);
                            }
                            break;

                        default:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                LocationList = LocationList.OrderBy(x => x.CreatedDate);
                            }
                            else
                            {
                                LocationList = LocationList.OrderByDescending(x => x.CreatedDate);
                            }

                            break;
                    }
                    LocationList = LocationList.Skip<Location>((request.PageNo - 1) * request.PageSize).Take<Location>(request.PageSize);
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

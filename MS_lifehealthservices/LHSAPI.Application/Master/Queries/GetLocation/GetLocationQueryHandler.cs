
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

namespace LHSAPI.Application.Master.Queries.GetLocation
{
    public class GetLocationQueryHandler : IRequestHandler<GetLocationInfoQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetLocationQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetLocationInfoQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                //var LocationList = _dbContext.Location.Where(x => x.IsDeleted == false && x.IsActive == true
                //).ToList();
                var locationList = (from loc in _dbContext.Location
                                    where loc.IsActive == true && loc.IsDeleted == false
                                    select new
                                    {
                                        LocationId = loc.LocationId,
                                        Name = loc.Address,
                                        Address = loc.Name,
                                        ManagerId = loc.ManagerId,
                                        CreatedDate = loc.CreatedDate
                                    }).ToList();
                if (locationList != null && locationList.Any())
                {

                    response.SuccessWithOutMessage(locationList);

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











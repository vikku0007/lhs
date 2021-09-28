
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

namespace LHSAPI.Application.Administration.Queries.GetUserActivityLog
{
    public class GetUserActivityLogHandler : IRequestHandler<GetUserActivityLogQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;

        public GetUserActivityLogHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<ApiResponse> Handle(GetUserActivityLogQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                var AvbempList = (from activity in _dbContext.ActivityLog
                                  join emInfo in _dbContext.EmployeePrimaryInfo on activity.CreatedById equals emInfo.Id
                                  where activity.IsDeleted == false &&  (string.IsNullOrEmpty(request.SearchTextByName) || emInfo.FirstName.Contains(request.SearchTextByName) || emInfo.LastName.Contains(request.SearchTextByName))
                                  select new
                                  {
                                      Id = activity.Id,
                                      activity.EntityName,
                                      activity.Description,
                                      CreatedDate = activity.CreatedDate,
                                      CreatedBy= _dbContext.EmployeePrimaryInfo.Where(x => x.Id == activity.CreatedById).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault()
                                });
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                       
                        case Common.Enums.Employee.ActivityLogOrderBy.Description:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Description);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Description);
                            }
                            break;
                        case Common.Enums.Employee.ActivityLogOrderBy.CreatedBy:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.CreatedBy);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.CreatedBy);
                            }
                            break;
                        case Common.Enums.Employee.ActivityLogOrderBy.appliedDate:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.CreatedDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.CreatedDate);
                            }
                            break;
                        default:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.CreatedDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.CreatedDate);
                            }

                            break;
                    }

                    var clientlist = AvbempList.ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(clientlist);
 }
                else
                {
                    response = response.NotFound();
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

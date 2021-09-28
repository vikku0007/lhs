
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

namespace LHSAPI.Application.Administration.Queries.GetAllPublicHoliday
{
    public class GetAllPublicHolidayHandler : IRequestHandler<GetAllPublicHolidayQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetAllPublicHolidayHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }

        public async Task<ApiResponse> Handle(GetAllPublicHolidayQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                var AvbempList = (from holiday in _dbContext.PublicHoliday
                                  where holiday.IsDeleted == false
                                  && ((string.IsNullOrEmpty(request.searchTextByCodeDescription) || (holiday.Holiday.Contains(request.searchTextByCodeDescription))))
                                  select new
                                  {
                                      Id = holiday.Id,
                                      holiday.Holiday,
                                      holiday.DateFrom,
                                      holiday.DateTo,
                                      CreatedDate = holiday.CreatedDate,
                                      holiday.Year,
                                      YearName =  _dbContext.StandardCode.Where(x => x.ID == holiday.Year).Select(x => x.CodeDescription).FirstOrDefault(),


                                  });
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Employee.HolidayOrderBy.Year:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Year);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Year);
                            }
                            break;
                        case Common.Enums.Employee.HolidayOrderBy.Holiday:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Holiday);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Holiday);
                            }
                            break;
                        case Common.Enums.Employee.HolidayOrderBy.DateFrom:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.DateFrom);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.DateFrom);
                            }
                            break;
                        case Common.Enums.Employee.HolidayOrderBy.DateTo:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.DateTo);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.DateTo);
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

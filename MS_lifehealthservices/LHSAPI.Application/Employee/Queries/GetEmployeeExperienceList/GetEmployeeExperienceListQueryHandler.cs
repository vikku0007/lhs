
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

namespace LHSAPI.Application.Employee.Queries.GetEmployeeExperienceList
{
    public class GetEmployeeExperienceListQueryHandler : IRequestHandler<GetEmployeeExperienceListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeExperienceListQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
       
        public async Task<ApiResponse> Handle(GetEmployeeExperienceListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                var AvbempList = _dbContext.EmployeeWorkExp.Where(x => x.EmployeeId == request.EmployeeId && x.IsActive == true
                  && x.IsDeleted == false); 
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Employee.EmployeeExperienceOrderBy.Company:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Company);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Company);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeExperienceOrderBy.JobTitle:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.JobTitle);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.JobTitle);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeExperienceOrderBy.StarDate:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.StartDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.StartDate);
                            }
                            break;

                        case Common.Enums.Employee.EmployeeExperienceOrderBy.EndDate:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.EndDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.EndDate);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeExperienceOrderBy.JobDesc:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.JobDesc);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.JobDesc
                                );
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


                    //empList = empList.Skip<EmployeePrimaryInfo>((request.PageNo > 0 ? (request.PageNo - 1) : request.PageNo) * request.PageSize).Take<EmployeePrimaryInfo>(request.PageSize).ToList();
                    //  var clientlist = AvbempList.ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(AvbempList);



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

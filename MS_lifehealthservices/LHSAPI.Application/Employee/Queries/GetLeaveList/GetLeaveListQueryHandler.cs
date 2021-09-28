
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

namespace LHSAPI.Application.Employee.Queries.GetLeaveList
{
    public class GetLeaveListQueryHandler : IRequestHandler<GetLeaveListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetLeaveListQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetLeaveListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {


                //var commList = _dbContext.EmployeeLeaveInfo.Where(x => x.EmployeeId == request.EmployeeId && x.IsActive == true
                // && x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();

                var AvbempList = (from leave in _dbContext.EmployeeLeaveInfo
                                  where leave.IsActive == true && leave.IsDeleted == false && leave.EmployeeId == request.EmployeeId
                                  select new LHSAPI.Application.Employee.Models.EmployeeLeaveModel
                                  {
                                      Id = leave.Id,
                                      EmployeeId = leave.Id,
                                      DateFrom = leave.DateFrom,
                                      DateTo = leave.DateTo,
                                      LeaveType = leave.LeaveType,
                                      ReasonOfLeave = leave.ReasonOfLeave,
                                      LeaveTypeName = _dbContext.StandardCode.Where(x => x.ID == leave.LeaveType).Select(x => x.CodeDescription).FirstOrDefault(),
                                      CreatedDate = leave.CreatedDate,
                                      IsApproved = leave.IsApproved,
                                      IsRejected = leave.IsRejected
                                  });



                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {

                        case Common.Enums.Employee.EmployeeLeaveInfoOrderBy.LeaveType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.LeaveType);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.LeaveType);
                            }
                            break;

                        case Common.Enums.Employee.EmployeeLeaveInfoOrderBy.DateFrom:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.DateFrom);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.DateFrom);
                            }
                            break;

                        case Common.Enums.Employee.EmployeeLeaveInfoOrderBy.DateTo:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.DateTo);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.DateTo);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeLeaveInfoOrderBy.Reason:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.ReasonOfLeave);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.ReasonOfLeave);
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
        #endregion
    }
}

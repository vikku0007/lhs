
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

namespace LHSAPI.Application.EmployeeStaff.Queries.GetEmployeeApplyLeave
{
    public class GetEmployeeApplyLeaveHandler : IRequestHandler<GetEmployeeApplyLeaveQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeApplyLeaveHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetEmployeeApplyLeaveQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                //var empList = _dbContext.EmployeePrimaryInfo.Where(x => x.IsDeleted == false && x.IsActive).OrderByDescending(x => x.Id).ToList();
                var AvbempList = (from emp in _dbContext.EmployeeJobProfile
                               join leave in _dbContext.EmployeeLeaveInfo on emp.EmployeeId equals leave.EmployeeId
                               where emp.IsActive == true && emp.IsDeleted == false && emp.ReportingToId == request.EmployeeId
                               select new
                               {
                                   Id = emp.ReportingToId,
                                   FullName = _dbContext.EmployeePrimaryInfo.Where(x => x.Id == emp.EmployeeId).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                                   DateFrom = leave.DateFrom,
                                   DateTo = leave.DateTo,
                                   LeaveType = leave.LeaveType,
                                   ReasonOfLeave = leave.ReasonOfLeave,
                                   LeaveTypeName = _dbContext.StandardCode.Where(x => x.ID == leave.LeaveType).Select(x => x.CodeDescription).FirstOrDefault(),
                                   CreatedDate = leave.CreatedDate,
                                   LeaveId=leave.Id,
                                   IsApproved=leave.IsApproved,
                                   IsRejected=leave.IsRejected,
                                   EmployeeId=leave.EmployeeId,
                                   Reason=leave.ReasonOfLeave
                               });

                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Employee.ApplyLeaveInfoOrderBy.FullName:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.FullName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.FullName);
                            }
                            break;

                        case Common.Enums.Employee.ApplyLeaveInfoOrderBy.LeaveType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.LeaveType);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.LeaveType);
                            }
                            break;

                        case Common.Enums.Employee.ApplyLeaveInfoOrderBy.DateFrom:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.DateFrom);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.DateFrom);
                            }
                            break;

                        case Common.Enums.Employee.ApplyLeaveInfoOrderBy.DateTo:
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

                    var leavelist = AvbempList.ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(leavelist);



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












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

namespace LHSAPI.Application.Employee.Queries.GetEmployeeAppraisalList
{
    public class GetEmployeeAppraisalListQueryHandler : IRequestHandler<GetEmployeeAppraisalListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeAppraisalListQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
       
        public async Task<ApiResponse> Handle(GetEmployeeAppraisalListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

               

                var AvbempList = (from staff in _dbContext.EmployeeAppraisalDetails
                                where staff.IsActive == true && staff.IsDeleted == false && staff.EmployeeId == request.EmployeeId
                                select new LHSAPI.Application.Employee.Models.EmployeeAppraisalTypemodel
                                {
                                    Id = staff.Id,
                                    EmployeeId = staff.Id,
                                    AppraisalDateFrom = staff.AppraisalDateFrom,
                                    AppraisalDateTo = staff.AppraisalDateTo,
                                    DepartmentName = staff.DepartmentName,
                                    AppraisalType = staff.AppraisalType,
                                    AppraisalTypeName = _dbContext.StandardCode.Where(x => x.ID == staff.AppraisalType).Select(x => x.CodeDescription).FirstOrDefault(),
                                    CreatedDate=staff.CreatedDate
                                });
                if (request.Id > 0)
                {
                   var Existstandard = _dbContext.EmployeeAppraisalStandards.Where(x => x.AppraisalId == request.Id && x.IsDeleted == false && x.IsActive == true).ToList();
                    var totalCount = Existstandard.Count;
                    response.ResponseData = Existstandard;
                    response.Total = totalCount;
                    response.Status = 1;
                }

                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                       
                        case Common.Enums.Employee.EmployeeAppraisalOrderBy.EmployeedetailId:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.EmployeeId);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.EmployeeId);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAppraisalOrderBy.AppraisalType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.AppraisalType);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.AppraisalType);
                            }
                            break;

                        case Common.Enums.Employee.EmployeeAppraisalOrderBy.AppraisalDateFrom:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.AppraisalDateFrom);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.AppraisalDateFrom);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeAppraisalOrderBy.AppraisalDateTo:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.AppraisalDateTo);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.AppraisalDateTo
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
        response.Status = (int)Number.Zero;
        response.Message = ResponseMessage.Error;
                response.StatusCode = HTTPStatusCode.INTERNAL_SERVER_ERROR;
            }
            return response;
        }
      
    }
}

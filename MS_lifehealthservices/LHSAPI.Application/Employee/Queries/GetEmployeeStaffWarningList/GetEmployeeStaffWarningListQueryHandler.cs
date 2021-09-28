
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

namespace LHSAPI.Application.Employee.Queries.GetEmployeeStaffWarningList
{
    public class GetEmployeeStaffWarningListQueryHandler : IRequestHandler<GetEmployeeStaffWarningListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeStaffWarningListQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }

        public async Task<ApiResponse> Handle(GetEmployeeStaffWarningListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                              var AvbempList = (from staff in _dbContext.EmployeeStaffWarning
                                where staff.IsActive == true && staff.IsDeleted == false && staff.EmployeeId == request.EmployeeId
                                select new LHSAPI.Application.Employee.Models.EmployeeStaffWarningModel
                                {
                                    Id = staff.Id,
                                    EmployeeId = staff.Id,
                                    WarningType = staff.WarningType,
                                    OffensesType = staff.OffensesType,
                                    Description = staff.Description,
                                    ImprovementPlan = staff.ImprovementPlan,
                                    Warning = _dbContext.StandardCode.Where(x => x.ID == staff.WarningType).Select(x => x.CodeDescription).FirstOrDefault(),
                                    OffensesTypeName = _dbContext.StandardCode.Where(x => x.ID == staff.OffensesType).Select(x => x.CodeDescription).FirstOrDefault(),
                                    CreatedDate = staff.CreatedDate,
                                    OtherOffenses=staff.OtherOffenses
                                });

                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                       
                        case Common.Enums.Employee.EmployeeStaffInfoOrderBy.WarningType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.WarningType);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.WarningType);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeStaffInfoOrderBy.offensestype:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.OffensesTypeName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.OffensesTypeName);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeStaffInfoOrderBy.Description:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Description);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Description);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeStaffInfoOrderBy.ImprovementPlan:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.ImprovementPlan);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.ImprovementPlan);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeStaffInfoOrderBy.otheroffenses:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.OtherOffenses);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.OtherOffenses);
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

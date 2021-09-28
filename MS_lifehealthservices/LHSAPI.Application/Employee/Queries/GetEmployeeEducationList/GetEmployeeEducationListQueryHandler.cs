
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

namespace LHSAPI.Application.Employee.Queries.GetEmployeeEducationList
{
    public class GetEmployeeEducationListQueryHandler : IRequestHandler<GetEmployeeEducationListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeEducationListQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
       
        public async Task<ApiResponse> Handle(GetEmployeeEducationListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

              
                var AvbempList = (from emp in _dbContext.EmployeeEducation
                                  where emp.IsActive == true && emp.IsDeleted == false && emp.EmployeeId == request.EmployeeId
                                  select new LHSAPI.Application.Employee.Models.EmployeeEducationModel
                                  {
                                      Id = emp.Id,
                                      EmployeeId = emp.Id,
                                      Institute = emp.Institute,
                                      Degree = emp.Degree,
                                      FieldStudy = emp.FieldStudy,
                                      CompletionDate = emp.CompletionDate,
                                      AdditionalNotes = emp.AdditionalNotes,
                                      DocumentPath = emp.DocumentPath,
                                      QualificationType=emp.QualificationType,
                                      QualificationTypeName = _dbContext.StandardCode.Where(x => x.ID == emp.QualificationType).Select(x => x.CodeDescription).FirstOrDefault(),
                                      CreatedDate=emp.CreatedDate
                                     
                                     
                                  });
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Employee.EmployeeEducationOrderBy.QualificationType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.QualificationType);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.QualificationType);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeEducationOrderBy.Institute:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Institute);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Institute);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeEducationOrderBy.Degree:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Degree);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Degree);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeEducationOrderBy.FieldStudy:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.FieldStudy);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.FieldStudy);
                            }
                            break;

                        case Common.Enums.Employee.EmployeeEducationOrderBy.CompletionDate:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.CompletionDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.CompletionDate);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeEducationOrderBy.AdditionalNotes:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.AdditionalNotes);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.AdditionalNotes
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

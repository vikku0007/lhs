
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

namespace LHSAPI.Application.Employee.Queries.GetEmployeeTrainingList
{
    public class GetEmployeeTrainingListHandler : IRequestHandler<GetEmployeeTrainingListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeTrainingListHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetEmployeeTrainingListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
               var AvbempList = (from emp in _dbContext.EmployeeTraining
                                                     where emp.IsActive == true && emp.IsDeleted == false && emp.EmployeeId == request.EmployeeId
                                                     select new LHSAPI.Application.Employee.Models.EmployeeTraining
                                                     {
                                                         Id = emp.Id,
                                                         EmployeeId = emp.Id,
                                                         MandatoryTraining = emp.MandatoryTraining,
                                                         TrainingType = emp.TrainingType,
                                                         CourseType = emp.CourseType,
                                                         StartDate = emp.StartDate,
                                                         EndDate = emp.EndDate,
                                                         Remarks = emp.Remarks,
                                                         CourseTypeName = _dbContext.StandardCode.Where(x => x.ID == emp.CourseType).Select(x => x.CodeDescription).FirstOrDefault(),
                                                         MandatoryName = _dbContext.StandardCode.Where(x => x.ID == emp.MandatoryTraining).Select(x => x.CodeDescription).FirstOrDefault(),
                                                         TrainingTypeName = _dbContext.StandardCode.Where(x => x.ID == emp.TrainingType).Select(x => x.CodeDescription).FirstOrDefault(),
                                                         IsAlert=emp.IsAlert,
                                                         CreatedDate=emp.CreatedDate,
                                                         FileName=emp.FileName,
                                                         OtherTraining=emp.OtherTraining
                                                     });


                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Employee.EmployeeTrainingOrderBy.TrainingType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.TrainingType);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.TrainingType);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeTrainingOrderBy.Training:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.MandatoryTraining);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.MandatoryTraining);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeTrainingOrderBy.CourseType:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.CourseType);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.CourseType);
                            }
                            break;

                        case Common.Enums.Employee.EmployeeTrainingOrderBy.StartDate:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.StartDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.StartDate);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeTrainingOrderBy.EndDate:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.EndDate);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.EndDate
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

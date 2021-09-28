
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
using System.Security.Cryptography;
using LHSAPI.Application.Employee.Models;
using System.IO;

namespace LHSAPI.Application.Employee.Queries
{
    public class GetEmployeeListHandler : IRequestHandler<GetEmployeeList, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeListHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetEmployeeList request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                //var empList = _dbContext.EmployeePrimaryInfo.Where(x => x.IsDeleted == false && x.IsActive).OrderByDescending(x => x.Id).ToList();
                var empList = (from emp in _dbContext.EmployeePrimaryInfo
                               join empj in _dbContext.EmployeeJobProfile on emp.Id equals empj.EmployeeId into empjt
                               from subempj in empjt.DefaultIfEmpty()
                                   //join loc in _dbContext.Location on subempj.LocationId equals loc.LocationId into gj
                                   //from subpet in gj.DefaultIfEmpty()
                                   // where emp.IsActive == true && emp.IsDeleted == false &&
                               where emp.IsDeleted == false && (string.IsNullOrEmpty(request.SearchTextByName) || emp.FirstName.Contains(request.SearchTextByName) || emp.LastName.Contains(request.SearchTextByName))
                               select new EmployeePrimaryInfoViewModel
                               {
                                   Id = emp.Id,
                                   Saluation = emp.Saluation,
                                   FirstName = emp.FirstName,
                                   LastName = emp.LastName,
                                   MiddleName = emp.MiddleName,
                                   Role = emp.Role,
                                   MaritalStatus = emp.MaritalStatus,
                                   MobileNo = emp.MobileNo,
                                   Gender = emp.Gender,
                                   EmailId = emp.EmailId,
                                   EmployeeId = emp.EmployeeId,
                                   EmployeeLevel = emp.EmployeeLevel,
                                   Status = emp.Status,
                                   Country = emp.Country,
                                   State = emp.State,
                                   City = emp.City,
                                   Address1 = emp.Address1,
                                   DateOfBirth = emp.DateOfBirth,
                                   Age = DateTime.Now.Year - Convert.ToDateTime(emp.DateOfBirth).Year,
                                   ImageUrl = _dbContext.EmployeePicInfo.Where(x => x.EmployeeId == emp.Id).Any() ? _dbContext.EmployeePicInfo.Where(x => x.EmployeeId == emp.Id).Select(x => x.Path).FirstOrDefault() : null,
                                   DateOfJoining = subempj.DateOfJoining ?? null,
                                   // LocationId = subpet.LocationId,
                                   FullName = emp.FirstName + " " + ((emp.MiddleName == null) ? "" : " " + emp.MiddleName) + " " + ((emp.LastName == null) ? "" : " " + emp.LastName),
                                   GenderName = _dbContext.StandardCode.Where(x => x.ID == emp.Gender).Select(x => x.CodeDescription).FirstOrDefault(),
                                   CreatedDate = emp.CreatedDate
                               });


                if (empList != null && empList.Any())
                {
                    var totalCount = empList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Employee.EmployeeOrderBy.Name:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                empList = empList.OrderBy(x => x.FullName);
                            }
                            else
                            {
                                empList = empList.OrderByDescending(x => x.FullName);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeOrderBy.Email:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                empList = empList.OrderBy(x => x.EmailId);
                            }
                            else
                            {
                                empList = empList.OrderByDescending(x => x.EmailId);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeOrderBy.MobileNo:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                empList = empList.OrderBy(x => x.MobileNo);
                            }
                            else
                            {
                                empList = empList.OrderByDescending(x => x.MobileNo);
                            }
                            break;
                        case Common.Enums.Employee.EmployeeOrderBy.DateOfEmployment:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                empList = empList.OrderBy(x => x.DateOfJoining);
                            }
                            else
                            {
                                empList = empList.OrderByDescending(x => x.DateOfJoining);
                            }
                            break;
                        default:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                empList = empList.OrderBy(x => x.CreatedDate);
                            }
                            else
                            {
                                empList = empList.OrderByDescending(x => x.CreatedDate);
                            }

                            break;
                    }


                    //empList = empList.Skip<EmployeePrimaryInfo>((request.PageNo > 0 ? (request.PageNo - 1) : request.PageNo) * request.PageSize).Take<EmployeePrimaryInfo>(request.PageSize).ToList();
                    var empList1 = empList.ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(empList1);
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


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

namespace LHSAPI.Application.Employee.Queries.GetAllEmployeeCommunicationList
{
    public class GetAllEmployeeCommunicationListHandler : IRequestHandler<GetAllEmployeeCommunicationListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetAllEmployeeCommunicationListHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetAllEmployeeCommunicationListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            List<LHSAPI.Application.Employee.Models.EmployeeCommunicationModel> AvbempList = new List<LHSAPI.Application.Employee.Models.EmployeeCommunicationModel>();
            try
            {

                var list = (from Employeedata in _dbContext.EmployeePrimaryInfo
                                  join RequireComp in _dbContext.EmployeeCommunicationInfo on Employeedata.Id equals RequireComp.EmployeeId
                                  where RequireComp.IsDeleted == false && RequireComp.IsActive == true && Employeedata.IsDeleted == false && Employeedata.IsActive == true && (string.IsNullOrEmpty(request.SearchTextByName) || Employeedata.FirstName.Contains(request.SearchTextByName) || Employeedata.LastName.Contains(request.SearchTextByName))
                            select new
                                  {
                                      RequireComp,
                                      Id=RequireComp.Id,
                                      Employeedata.FirstName,
                                      Employeedata.MiddleName,
                                      Employeedata.LastName,
                                      FullName = Employeedata.FirstName + " " + ((Employeedata.MiddleName == null) ? "" : " " + Employeedata.MiddleName) + " " + ((Employeedata.LastName == null) ? "" : " " + Employeedata.LastName),
                                     // AssignedToName = _dbContext.EmployeePrimaryInfo.Where(x => x.Id == RequireComp.AssignedTo).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                                  }).OrderByDescending(x => x.Id).ToList();
                foreach (var item in list)
                {
                    LHSAPI.Application.Employee.Models.EmployeeCommunicationModel comm = new LHSAPI.Application.Employee.Models.EmployeeCommunicationModel()
                    {
                        Id = item.Id,
                        EmployeeId = item.RequireComp.EmployeeId,
                        FullName=item.FullName,
                        Subject = item.RequireComp.Subject,
                        Message = item.RequireComp.Message,
                        CreatedDate = item.RequireComp.CreatedDate,
                       
                    };
                    comm.CommunicationRecepientmodel = (from comminfo in _dbContext.EmployeeCommunicationInfo
                                                        join recepient in _dbContext.CommunicationRecipient on comminfo.Id equals recepient.CommunicationId
                                                        join emInfo in _dbContext.EmployeePrimaryInfo on recepient.EmployeeId equals emInfo.Id

                                                        where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.Id == item.Id
                                                        select new LHSAPI.Application.Employee.Models.CommunicationRecepientmodel
                                                        {
                                                            Id = comminfo.Id,
                                                            EmployeeId = emInfo.Id,
                                                            CommunicationId = recepient.CommunicationId,
                                                            AssignedToName = emInfo.FirstName + " " + (emInfo.MiddleName == null ? "" : emInfo.MiddleName) + " " + emInfo.LastName,
                                                        }).ToList();

                    AvbempList.Add(comm);

                }
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Employee.EmployeeCommunicationOrderBy.Name:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.FullName).ToList();
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.FullName).ToList();
                            }
                            break;
                        case Common.Enums.Employee.EmployeeCommunicationOrderBy.Subject:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Subject).ToList();
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Subject).ToList();
                            }
                            break;
                        case Common.Enums.Employee.EmployeeCommunicationOrderBy.DateTime:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.CreatedDate).ToList();
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.CreatedDate).ToList();
                            }
                            break;

                       
                        default:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.CreatedDate).ToList();
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.CreatedDate).ToList();
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

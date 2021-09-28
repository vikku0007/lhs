
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

namespace LHSAPI.Application.Employee.Queries.GetEmployeeCommunicationList
{
    public class GetEmployeeCommunicationListQueryHandler : IRequestHandler<GetEmployeeCommunicationListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetEmployeeCommunicationListQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetEmployeeCommunicationListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            List<LHSAPI.Application.Employee.Models.EmployeeCommunicationModel> list = new List<LHSAPI.Application.Employee.Models.EmployeeCommunicationModel>();
            try
            {
                var commList = (from accident in _dbContext.EmployeeCommunicationInfo
                                where accident.IsActive == true && accident.IsDeleted == false && accident.EmployeeId == request.EmployeeId
                                select new LHSAPI.Application.Employee.Models.EmployeeCommunicationModel
                                {
                                    Id = accident.Id,
                                    EmployeeId = accident.Id,
                                    Subject = accident.Subject,
                                    Message = accident.Message,
                                   // AssignedToName = _dbContext.EmployeePrimaryInfo.Where(x => x.Id == accident.AssignedTo).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                                    CreatedDate = accident.CreatedDate
                                }).OrderByDescending(x => x.Id).ToList();
                foreach (var item in commList)
                {
                    LHSAPI.Application.Employee.Models.EmployeeCommunicationModel comm = new LHSAPI.Application.Employee.Models.EmployeeCommunicationModel()
                    {
                        Id = item.Id,
                        EmployeeId = item.EmployeeId,
                        Subject = item.Subject,
                        Message = item.Message,
                        CreatedDate = item.CreatedDate,
                      
                    };
                    comm.CommunicationRecepientmodel = (from comminfo in _dbContext.EmployeeCommunicationInfo
                                                        join recepient in _dbContext.CommunicationRecipient on comminfo.Id equals recepient.CommunicationId
                                                        join emInfo in _dbContext.EmployeePrimaryInfo on recepient.EmployeeId equals emInfo.Id

                                                        where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.Id == item.Id
                                                        select new LHSAPI.Application.Employee.Models.CommunicationRecepientmodel
                                                        {
                                                            Id = comminfo.Id,
                                                            EmployeeId = emInfo.Id,
                                                            CommunicationId=recepient.CommunicationId,
                                                            AssignedToName = emInfo.FirstName + " " + (emInfo.MiddleName == null ? "" : emInfo.MiddleName) + " " + emInfo.LastName,
                                                        }).OrderByDescending(x => x.Id).ToList();

                    list.Add(comm);

                }
                if (list != null && list.Any())
                {
                    var totalCount = list.Count();

                    switch (request.OrderBy)
                    {
                       
                        case Common.Enums.Employee.EmployeeCommunicationInfoOrderBy.Subject:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                list = list.OrderBy(x => x.Subject).ToList();
                            }
                            else
                            {
                                list = list.OrderByDescending(x => x.Subject).ToList();
                            }
                            break;
                        case Common.Enums.Employee.EmployeeCommunicationInfoOrderBy.DateTime:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                list = list.OrderBy(x => x.CreatedDate).ToList();
                            }
                            else
                            {
                                list = list.OrderByDescending(x => x.CreatedDate).ToList();
                            }
                            break;
                        case Common.Enums.Employee.EmployeeCommunicationInfoOrderBy.Message:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                list = list.OrderBy(x => x.Message).ToList();
                            }
                            else
                            {
                                list = list.OrderByDescending(x => x.Message).ToList();
                            }
                            break;


                        default:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                list = list.OrderBy(x => x.CreatedDate).ToList();
                            }
                            else
                            {
                                list = list.OrderByDescending(x => x.CreatedDate).ToList();
                            }

                            break;
                    }


                    //empList = empList.Skip<EmployeePrimaryInfo>((request.PageNo > 0 ? (request.PageNo - 1) : request.PageNo) * request.PageSize).Take<EmployeePrimaryInfo>(request.PageSize).ToList();
                    var clientlist = list.ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
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


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

namespace LHSAPI.Application.Client.Queries.GetOtherContactPersonInfo
{
    public class GetOtherContactPersonInfoHandler : IRequestHandler<GetOtherContactPersonInfoQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetOtherContactPersonInfoHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
       
        public async Task<ApiResponse> Handle(GetOtherContactPersonInfoQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                
              var  AvbempList = (from emp in _dbContext.ClientPrimaryCareInfo
                                                        where emp.IsActive == true && emp.IsDeleted == false && emp.ClientId == request.ClientId
                                                        select new LHSAPI.Application.Client.Models.ClientPrimaryCareInfo
                                                        {
                                                            Id = emp.Id,
                                                            ClientId = emp.ClientId,
                                                            Name = emp.FirstName + " " +
                                    (emp.MiddleName == null ? "" : emp.MiddleName) + " " + emp.LastName,
                                                            RelationShip = emp.RelationShip,
                                                            ContactNo = emp.ContactNo,
                                                            Email = emp.Email,
                                                            PhoneNo = emp.PhoneNo=="null"?"":emp.PhoneNo,
                                                            RelationShipName = _dbContext.StandardCode.Where(x => x.ID == emp.RelationShip).Select(x => x.CodeDescription).FirstOrDefault(),
                                                            MiddleName = emp.MiddleName,
                                                            LastName = emp.LastName,
                                                            FirstName = emp.FirstName,
                                                            Gender = emp.Gender,
                                                            CreatedDate=emp.CreatedDate,
                                                           OtherRelation = emp.OtherRelation,
                                                        });

                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Client.ClientContactOrderBy.Name:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Name);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Name);
                            }
                            break;
                        case Common.Enums.Client.ClientContactOrderBy.Relationship:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.RelationShip);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.RelationShip);
                            }
                            break;
                        case Common.Enums.Client.ClientContactOrderBy.Email:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Email
                                );
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Email);
                            }
                            break;

                        case Common.Enums.Client.ClientContactOrderBy.ContactNo:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.ContactNo);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.ContactNo);
                            }
                            break;
                        case Common.Enums.Client.ClientContactOrderBy.PhoneNo:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.PhoneNo);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.PhoneNo
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

using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LHSAPI.Domain.Entities;

namespace LHSAPI.Application.Client.Queries.GetAllClientList
{
    public class AddClientListHandler : IRequestHandler<GetAllClientListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public AddClientListHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
        #region Client List

        public async Task<ApiResponse> Handle(GetAllClientListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                var AvbempList = (from Employeedata in _dbContext.ClientPrimaryInfo
                                  join loc in _dbContext.Location on Employeedata.LocationId equals loc.LocationId into gj
                                  from subpet in gj.DefaultIfEmpty()
                                  where Employeedata.IsDeleted == false  && (string.IsNullOrEmpty(request.SearchTextByName) || Employeedata.FirstName.Contains(request.SearchTextByName) || Employeedata.LastName.Contains(request.SearchTextByName)) && (string.IsNullOrEmpty(request.SearchTextBylocation) || (subpet != null && subpet.Name.Contains(request.SearchTextBylocation)))
                                  select new
                                  {
                                      Id = Employeedata.Id,
                                      FirstName = Employeedata.FirstName,
                                      LastName = Employeedata.LastName,
                                      Gender = Employeedata.Gender,
                                      DateOfBirth = Employeedata.DateOfBirth,
                                      EmailId = Employeedata.EmailId,
                                      MaritalStatus = Employeedata.MaritalStatus,
                                      Salutation = Employeedata.Salutation,
                                      LocationId = Employeedata.LocationId,
                                      MiddleName = Employeedata.MiddleName,
                                      MobileNo = Employeedata.MobileNo,
                                      ClientId = Employeedata.ClientId,
                                      Address = Employeedata.Address,
                                      Name = _dbContext.Location.Where(x => x.LocationId == Employeedata.LocationId).Select(x => x.Name).FirstOrDefault(),
                                      GenderName = _dbContext.StandardCode.Where(x => x.ID == Employeedata.Gender).Select(x => x.CodeDescription).FirstOrDefault(),
                                      FullName = Employeedata.FirstName + " " +
                                      (Employeedata.MiddleName == null ? "" : Employeedata.MiddleName) + " " + Employeedata.LastName,
                                      ImageUrl = _dbContext.ClientPicInfo.Where(x => x.ClientId == Employeedata.Id).Any() ? _dbContext.ClientPicInfo.Where(x => x.ClientId == Employeedata.Id).Select(x => x.Path).FirstOrDefault() : null,
                                      CreatedDate = Employeedata.CreatedDate,
                                      IsActive=Employeedata.IsActive,
                                      //status = Employeedata.Status
                                  });
               
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                        switch (request.OrderBy)
                        {
                            case Common.Enums.Client.ClientOrderBy.Name:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                AvbempList = AvbempList.OrderBy(x => x.FullName);
                                }
                                else
                                {
                                AvbempList = AvbempList.OrderByDescending(x => x.FullName);
                                }
                                break;
                            case Common.Enums.Client.ClientOrderBy.Email:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                AvbempList = AvbempList.OrderBy(x => x.EmailId);
                                }
                                else
                                {
                                AvbempList = AvbempList.OrderByDescending(x => x.EmailId);
                                }
                                break;
                        case Common.Enums.Client.ClientOrderBy.MobileNo:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                AvbempList = AvbempList.OrderBy(x => x.MobileNo);
                                }
                                else
                                {
                                AvbempList = AvbempList.OrderByDescending(x => x.MobileNo);
                                }
                                break;
                        case Common.Enums.Client.ClientOrderBy.Address:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                AvbempList = AvbempList.OrderBy(x => x.Address);
                                }
                                else
                                {
                                AvbempList = AvbempList.OrderByDescending(x => x.Address
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

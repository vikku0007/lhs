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
using LHSAPI.Application.Shift.Models;

namespace LHSAPI.Application.Client.Queries.GetClientShiftList
{
    public class GetClientShiftListHandler : IRequestHandler<GetClientShiftListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetClientShiftListHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
        #region Get Shift List

        public async Task<ApiResponse> Handle(GetClientShiftListQuery request, CancellationToken cancellationToken)
        {
           
            ApiResponse response = new ApiResponse();
            try
            {
                // List<ShiftInfoViewModel> list = new List<ShiftInfoViewModel>();
                
                var list = (from clientshift in _dbContext.ClientShiftInfo 
                           join shiftdata in _dbContext.ShiftInfo on clientshift.ShiftId equals shiftdata.Id
                            join empshift in _dbContext.EmployeeShiftInfo on shiftdata.Id equals empshift.ShiftId
                            join emInfo in _dbContext.EmployeePrimaryInfo on empshift.EmployeeId equals emInfo.Id
                            where clientshift.IsDeleted == false && clientshift.IsActive == true && clientshift.ClientId == request.ClientId
                            select new ShiftInfoViewModel()
                            {
                                Id = shiftdata.Id,
                                Description = shiftdata.Description,
                                ClientCount = shiftdata.ClientCount,
                                EmployeeCount = shiftdata.EmployeeCount,
                                StartDate = shiftdata.StartDate.Date.Add(shiftdata.StartTime),
                                StartTime = shiftdata.StartTime,
                                EndDate = shiftdata.EndDate.Date.Add(shiftdata.EndTime),
                                EndTime = shiftdata.EndTime,
                                StartTimeString = shiftdata.StartDate.Date.Add(shiftdata.StartTime).ToString(@"hh\:mm tt"),
                                EndTimeString = shiftdata.EndDate.Date.Add(shiftdata.EndTime).ToString(@"hh\:mm tt"),
                                Duration = shiftdata.Duration,
                                CreatedDate=shiftdata.CreatedDate,
                                EmployeeId= _dbContext.EmployeeShiftInfo.Where(x => x.ShiftId == clientshift.ShiftId).Select(x => x.EmployeeId).FirstOrDefault(),
                                Name = _dbContext.EmployeePrimaryInfo.Where(x => x.Id == emInfo.Id).Select(x => x.FirstName + " " + ((x.MiddleName == null) ? "" : " " + x.MiddleName) + " " + ((x.LastName == null) ? "" : " " + x.LastName)).FirstOrDefault(),
                               
                            }); 
                  if (list != null && list.Any())
                {
                    switch (request.OrderBy)
                    {
                        case Common.Enums.Client.ClientShiftlistOrderBy.Name:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                list = list.OrderBy(x => x.Name);
                            }
                            else
                            {
                                list = list.OrderByDescending(x => x.Name);
                            }
                            break;
                       
                      
                        case Common.Enums.Client.ClientShiftlistOrderBy.StartTime:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                list = list.OrderBy(x => x.StartTime);
                            }
                            else
                            {
                                list = list.OrderByDescending(x => x.StartTime);
                            }
                            break;
                        case Common.Enums.Client.ClientShiftlistOrderBy.EndTime:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                list = list.OrderBy(x => x.EndTime);
                            }
                            else
                            {
                                list = list.OrderByDescending(x => x.EndTime);
                            }
                            break;
                        case Common.Enums.Client.ClientShiftlistOrderBy.Duration:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                list = list.OrderBy(x => x.Duration);
                            }
                            else
                            {
                                list = list.OrderByDescending(x => x.Duration);
                            }
                            break;
                        default:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                list = list.OrderBy(x => x.CreatedDate);
                            }
                            else
                            {
                                list = list.OrderByDescending(x => x.CreatedDate);
                            }

                            break;
                    }

                    var clientlist = list.ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                    var  totalCount = list.Count();
                    response.Total = totalCount;
                    response.SuccessWithOutMessage(clientlist);



                }
                else
                {
                    response = response.NotFound();
                }
                if (list != null && list.Any())
                {
                    var clientlist = list.ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                    var  totalCount = list.Count();
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

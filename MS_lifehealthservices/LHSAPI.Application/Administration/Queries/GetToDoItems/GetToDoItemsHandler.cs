
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

namespace LHSAPI.Application.Administration.Queries.GetToDoItems
{
    public class GetToDoItemsHandler : IRequestHandler<GetToDoItemsQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;

        public GetToDoItemsHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<ApiResponse> Handle(GetToDoItemsQuery request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {

                var AvbempList = (from todo in _dbContext.ToDoShiftItem
                                  where todo.IsDeleted == false && todo.IsActive == true
                                  && ((string.IsNullOrEmpty(request.searchTextByCodeDescription) || (todo.Description.Contains(request.searchTextByCodeDescription))))
                                  select new
                                  {
                                      Id = todo.Id,
                                      todo.Description,
                                      todo.ShiftType,
                                      CreatedDate = todo.CreatedDate,
                                      ShiftTypeName = _dbContext.StandardCode.Where(x => x.ID == todo.ShiftType).Select(x => x.CodeDescription).FirstOrDefault(),
                                  });
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Employee.ToDoOrderBy.Shifttype:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.ShiftTypeName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.ShiftTypeName);
                            }
                            break;
                        case Common.Enums.Employee.ToDoOrderBy.Description:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.Description);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.Description);
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

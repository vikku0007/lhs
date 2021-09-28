
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

namespace LHSAPI.Application.Administration.Queries.GetAllMasterEntries
{
    public class GetAllMasterEntriesListQueryHandler : IRequestHandler<GetAllMasterEntriesListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetAllMasterEntriesListQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
       
        public async Task<ApiResponse> Handle(GetAllMasterEntriesListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {

                var AvbempList = (from RequireComp in _dbContext.StandardCode

                                  where RequireComp.IsDeleted == false  && (string.IsNullOrEmpty(request.searchTextByCodeData) || RequireComp.CodeData.Contains(request.searchTextByCodeData)) && (string.IsNullOrEmpty(request.searchTextByCodeDescription) || (RequireComp.CodeDescription.Contains(request.searchTextByCodeDescription)))
                                  select new
                                  {
                                      
                                      Id = RequireComp.ID,
                                      RequireComp.CodeData,
                                      RequireComp.CodeDescription,
                                      CreatedDate = RequireComp.CreatedDate,
                                      IsActive=RequireComp.IsActive,
                                     
                                  });
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                        switch (request.OrderBy)
                        {
                            case Common.Enums.Employee.StandardCodeOrderBy.CodeData:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    AvbempList = AvbempList.OrderBy(x => x.CodeData);
                                }
                                else
                                {
                                    AvbempList = AvbempList.OrderByDescending(x => x.CodeData);
                                }
                                break;
                            case Common.Enums.Employee.StandardCodeOrderBy.CodeDescription:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    AvbempList = AvbempList.OrderBy(x => x.CodeDescription);
                                }
                                else
                                {
                                    AvbempList = AvbempList.OrderByDescending(x => x.CodeDescription);
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


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

using System.Text.Json;
using LHSAPI.Application.Client.Models;

namespace LHSAPI.Application.Client.Queries.GetClientProgressNotes
{
    public class GetClientProgressNotesQueryHandler : IRequestHandler<GetClientProgressNotesQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetClientProgressNotesQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetClientProgressNotesQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            
                try
                {
                var AvbempList = (from Clientdata in _dbContext.ClientPrimaryInfo
                                  join RequireComp in _dbContext.ProgressNotesList on Clientdata.Id equals RequireComp.ClientId
                                  where RequireComp.IsDeleted == false && RequireComp.IsActive == true && Clientdata.IsDeleted == false && Clientdata.IsActive == true && (string.IsNullOrEmpty(request.SearchTextByName) || Clientdata.FirstName.Contains(request.SearchTextByName) || Clientdata.LastName.Contains(request.SearchTextByName))
                                  select new
                                  {
                                      RequireComp,
                                      Id = RequireComp.Id,
                                      Clientdata.FirstName,
                                      Clientdata.MiddleName,
                                      Clientdata.LastName,
                                      FullName = Clientdata.FirstName + " " + ((Clientdata.MiddleName == null) ? "" : " " + Clientdata.MiddleName) + " " + ((Clientdata.LastName == null) ? "" : " " + Clientdata.LastName),
                                      CreatedDate = RequireComp.CreatedDate
                                  });
                if (AvbempList != null && AvbempList.Any())
                {
                    var totalCount = AvbempList.Count();

                    switch (request.OrderBy)
                    {
                        case Common.Enums.Client.ClientProgressOrderBy.Name:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.FullName);
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.FullName);
                            }
                            break;
                        //case Common.Enums.Client.ClientProgressOrderBy.ProgressNotes:
                        //    if (Common.Enums.SortOrder.Asc == request.SortOrder)
                        //    {
                        //        AvbempList = AvbempList.OrderBy(x => x.RequireComp.ProgressNote);
                        //    }
                        //    else
                        //    {
                        //        AvbempList = AvbempList.OrderByDescending(x => x.RequireComp.ProgressNote);
                        //    }
                        //    break;
                        case Common.Enums.Client.ClientProgressOrderBy.Date:
                            if (Common.Enums.SortOrder.Asc == request.SortOrder)
                            {
                                AvbempList = AvbempList.OrderBy(x => x.RequireComp.Date
                                );
                            }
                            else
                            {
                                AvbempList = AvbempList.OrderByDescending(x => x.RequireComp.Date);
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

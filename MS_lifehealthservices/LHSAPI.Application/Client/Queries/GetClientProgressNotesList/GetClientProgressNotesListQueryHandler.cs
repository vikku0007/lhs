
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

namespace LHSAPI.Application.Client.Queries.GetClientProgressNotesList
{
    public class GetClientProgressNotesListQueryHandler : IRequestHandler<GetClientProgressNotesListQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetClientProgressNotesListQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetClientProgressNotesListQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientDetails _clientDetails = new ClientDetails();
                List<ProgressNotesList> clientFundinglist = new List<ProgressNotesList>();
                var commList = _dbContext.ProgressNotesList.Where(x => x.ClientId == request.ClientId && x.IsActive == true 
                && x.ShiftId == request.ShiftId
                  && x.IsDeleted == false).AsQueryable().OrderByDescending(x => x.Id).ToList();
                if (commList != null && commList.Count > 0)
                {
                    
                        foreach (var item in commList)
                        {
                            ProgressNotesList clientFunding = new ProgressNotesList
                            {
                                Id = item.Id,
                                ClientProgressNoteId = (item.ClientProgressNoteId),
                                ClientId = item.ClientId,
                                Date = item.Date,
                                Note9AMTo11AM = item.Note9AMTo11AM,
                                Note11AMTo1PM = item.Note11AMTo1PM,
                                Note1PMTo15PM = item.Note1PMTo15PM,
                                Note15PMTo17PM =item.Note15PMTo17PM,
                                Note17PMTo19PM =item.Note17PMTo19PM,
                                Note19PMTo21PM =item.Note19PMTo21PM,
                                Note21PMTo23PM =item.Note21PMTo23PM,
                                Note23PMTo1AM =   item.Note23PMTo1AM,
                                Note1AMTo3AM =  item.Note1AMTo3AM,
                                Note3AMTo5AM = item.Note3AMTo5AM,
                                Note5AMTo7AM = item.Note5AMTo7AM,
                                Note7AMTo9AM = item.Note7AMTo9AM,
                                CreatedDate = item.CreatedDate,
                                Summary = item.Summary,
                                OtherInfo = item.OtherInfo,
                               
                            };
                        clientFundinglist.Add(clientFunding);
                        
                        }

                    if (clientFundinglist != null && clientFundinglist.Any())
                    {
                        var totalCount = clientFundinglist.Count();

                        switch (request.OrderBy)
                        {
                            
                            case Common.Enums.Client.ClientProgressInfoOrderBy.ProgressNotes:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.ProgressNote).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.ProgressNote).ToList();
                                }
                                break;
                            case Common.Enums.Client.ClientProgressInfoOrderBy.Date:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.Date).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.Date).ToList();
                                }
                                break;

                            default:
                                if (Common.Enums.SortOrder.Asc == request.SortOrder)
                                {
                                    clientFundinglist = clientFundinglist.OrderBy(x => x.CreatedDate).ToList();
                                }
                                else
                                {
                                    clientFundinglist = clientFundinglist.OrderByDescending(x => x.CreatedDate).ToList();
                                }

                                break;
                        }


                        //empList = empList.Skip<EmployeePrimaryInfo>((request.PageNo > 0 ? (request.PageNo - 1) : request.PageNo) * request.PageSize).Take<EmployeePrimaryInfo>(request.PageSize).ToList();
                        var clientlist = clientFundinglist.ToList().Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToList();
                        response.Total = totalCount;
                        response.SuccessWithOutMessage(clientlist);



                    }
                    else
                    {
                        response = response.NotFound();
                    }
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

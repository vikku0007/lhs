
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
using LHSAPI.Application.Employee.Models;
using static LHSAPI.Common.Enums.ResponseEnums;
using LHSAPI.Application.Client.Models;
using AutoMapper;

namespace LHSAPI.Application.Client.Queries.GetClientIncidentCategory
{
    public class GetClientIncidentCategoryHandler : IRequestHandler<GetClientIncidentCategoryQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetClientIncidentCategoryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetClientIncidentCategoryQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientAccidentIncidentCategory _clientDetails = new ClientAccidentIncidentCategory();


                _clientDetails.ClientIncidentCategory = (from accident in _dbContext.ClientIncidentCategory
                                                         where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId 
                                                         select new LHSAPI.Application.Client.Models.ClientIncidentCategory
                                                         {
                                                             Id = accident.Id,
                                                             ClientId = accident.ClientId,
                                                             IsIncidentAnticipated = accident.IsIncidentAnticipated

                                                         }).FirstOrDefault();


                _clientDetails.ClientPrimaryIncidentCategory = (from comminfo in _dbContext.ClientPrimaryIncidentCategory
                                                                where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                                select new LHSAPI.Application.Client.Models.ClientPrimaryIncidentCategory
                                                                {
                                                                    Id = comminfo.Id,
                                                                    ClientId = comminfo.ClientId,
                                                                    PrimaryIncidentId = comminfo.PrimaryIncidentId,
                                                                    PrimaryIncidentName = _dbContext.StandardCode.Where(x => x.ID == comminfo.PrimaryIncidentId).Select(x => x.CodeDescription).FirstOrDefault(),
                                                                }).OrderByDescending(x => x.Id).ToList();
                _clientDetails.ClientSecondaryIncidentCategory = (from comminfo in _dbContext.ClientSecondaryIncidentCategory
                                                                  where comminfo.IsDeleted == false && comminfo.IsActive == true && comminfo.ClientId == request.Id && comminfo.ShiftId == request.ShiftId
                                                                  select new LHSAPI.Application.Client.Models.ClientSecondaryIncidentCategory
                                                                  {
                                                                      Id = comminfo.Id,
                                                                      ClientId = comminfo.ClientId,
                                                                      SecondaryIncidentId = comminfo.SecondaryIncidentId,
                                                                      SecondaryIncidentName = _dbContext.StandardCode.Where(x => x.ID == comminfo.SecondaryIncidentId).Select(x => x.CodeDescription).FirstOrDefault(),
                                                                  }).OrderByDescending(x => x.Id).ToList();
               
                
                if (_clientDetails.ClientIncidentCategory == null) _clientDetails.ClientIncidentCategory = new LHSAPI.Application.Client.Models.ClientIncidentCategory();
                if (_clientDetails.ClientPrimaryIncidentCategory == null) _clientDetails.ClientPrimaryIncidentCategory = new List<LHSAPI.Application.Client.Models.ClientPrimaryIncidentCategory>();
                if (_clientDetails.ClientSecondaryIncidentCategory == null) _clientDetails.ClientSecondaryIncidentCategory = new List<LHSAPI.Application.Client.Models.ClientSecondaryIncidentCategory>();
                
                response.SuccessWithOutMessage(_clientDetails);
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

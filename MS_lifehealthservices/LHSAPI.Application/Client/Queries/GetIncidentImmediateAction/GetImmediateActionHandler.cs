
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

namespace LHSAPI.Application.Client.Queries.GetIncidentImmediateAction
{
    public class GetImmediateActionHandler : IRequestHandler<GetImmediateActionQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetImmediateActionHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetImmediateActionQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                ClientImmediateAction _clientDetails = new ClientImmediateAction();
                _clientDetails.IncidentImmediateAction = (from accident in _dbContext.IncidentImmediateAction
                                                          where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                          select new LHSAPI.Application.Client.Models.IncidentImmediateAction
                                                          {
                                                              Id = accident.Id,
                                                              ClientId = accident.ClientId,
                                                              IsPoliceInformed = accident.IsPoliceInformed,
                                                              OfficerName = accident.OfficerName,
                                                              PoliceStation = accident.PoliceStation,
                                                              PoliceNo = accident.PoliceNo,
                                                              ProviderPosition = accident.ProviderPosition,
                                                              PhoneNo = accident.PhoneNo,
                                                              IsFamilyAware = accident.IsFamilyAware,
                                                              ContacttoFamily = accident.ContacttoFamily,
                                                              IsUnder18 = accident.IsUnder18,
                                                              ContactChildProtection = accident.ContactChildProtection,
                                                              DisabilityPerson = accident.DisabilityPerson,
                                                              SubjectWorkerAllegation = accident.SubjectWorkerAllegation,
                                                              SubjectDisabilityPerson = accident.SubjectDisabilityPerson

                                                          }).FirstOrDefault();
                if (_clientDetails.IncidentImmediateAction == null) _clientDetails.IncidentImmediateAction = new LHSAPI.Application.Client.Models.IncidentImmediateAction();
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


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

namespace LHSAPI.Application.Client.Queries.GetRiskAssesment
{
    public class GetRiskAssesmentHandler : IRequestHandler<GetRiskAssesmentQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetRiskAssesmentHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetRiskAssesmentQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientRiskAssesment _clientDetails = new ClientRiskAssesment();

                _clientDetails.IncidentRiskAssesment = (from accident in _dbContext.IncidentRiskAssesment
                                                        where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                        select new LHSAPI.Application.Client.Models.IncidentRiskAssesment
                                                        {
                                                            Id = accident.Id,
                                                            ClientId = accident.ClientId,
                                                            IsRiskAssesment = accident.IsRiskAssesment,
                                                            RiskAssesmentDate = accident.RiskAssesmentDate,
                                                            RiskDetails = accident.RiskDetails,
                                                            NoRiskAssesmentInfo = accident.NoRiskAssesmentInfo,
                                                            InProgressRisk = accident.InProgressRisk,
                                                            TobeFinished = accident.TobeFinished
                                                        }).FirstOrDefault();
                if (_clientDetails.IncidentRiskAssesment == null) _clientDetails.IncidentRiskAssesment = new LHSAPI.Application.Client.Models.IncidentRiskAssesment();
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

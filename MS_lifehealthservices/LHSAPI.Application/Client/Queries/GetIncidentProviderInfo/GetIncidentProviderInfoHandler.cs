
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

namespace LHSAPI.Application.Client.Queries.GetIncidentProviderInfo
{
    public class GetIncidentProviderInfoHandler : IRequestHandler<GetIncidentProviderInfoQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetIncidentProviderInfoHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetIncidentProviderInfoQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientProviderDetails _clientDetails = new ClientProviderDetails();

                _clientDetails.ClientAccidentProviderInfo = (from client in _dbContext.ClientAccidentProviderInfo
                                                             where client.IsActive == true && client.IsDeleted == false && client.ClientId == request.Id && client.ShiftId == request.ShiftId
                                                             select new LHSAPI.Application.Client.Models.ClientAccidentProviderInfo
                                                             {
                                                                 Id = client.Id,
                                                                 ClientId = client.ClientId,
                                                                 ReportCompletedBy = client.ReportCompletedBy,
                                                                 ProviderName = client.ProviderName,
                                                                 ProviderregistrationId = client.ProviderregistrationId,
                                                                 ProviderABN = client.ProviderABN,
                                                                 OutletName = client.OutletName,
                                                                 Registrationgroup = client.Registrationgroup,
                                                                 State = client.State,
                                                                 StateName = _dbContext.StandardCode.Where(x => x.ID == client.State).Select(x => x.CodeDescription).FirstOrDefault(),
                                                             }).FirstOrDefault();

                if (_clientDetails.ClientAccidentProviderInfo == null)
                {
                    LHSAPI.Application.Client.Models.ClientAccidentProviderInfo provider = new LHSAPI.Application.Client.Models.ClientAccidentProviderInfo();
                    provider.Id = 0;
                    provider.ClientId = 0;
                    provider.ReportCompletedBy =
                    provider.ProviderName = "Life Health Services";
                    provider.ProviderregistrationId = "4-433C-2205";
                    provider.ProviderABN = "72 623 159 446";
                    provider.OutletName = null;
                    provider.Registrationgroup = null;
                    provider.State = 0;
                    provider.StateName = null;
                    _clientDetails.ClientAccidentProviderInfo = provider;
                }
                else
                {
                    _clientDetails.ClientAccidentProviderInfo = _clientDetails.ClientAccidentProviderInfo;
                }
               
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

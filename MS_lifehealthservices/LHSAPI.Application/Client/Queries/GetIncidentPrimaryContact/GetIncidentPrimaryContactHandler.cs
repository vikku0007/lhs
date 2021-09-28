
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

namespace LHSAPI.Application.Client.Queries.GetIncidentPrimaryContact
{
    public class GetIncidentPrimaryContactHandler : IRequestHandler<GetIncidentPrimaryContactQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetIncidentPrimaryContactHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetIncidentPrimaryContactQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientPrimaryContact _clientDetails = new ClientPrimaryContact();

                _clientDetails.ClientAccidentPrimaryContact = (from client in _dbContext.ClientAccidentPrimaryContact
                                                               where client.IsActive == true && client.IsDeleted == false && client.ClientId == request.Id && client.ShiftId == request.ShiftId
                                                               select new LHSAPI.Application.Client.Models.ClientAccidentPrimaryContact
                                                               {
                                                                   Id = client.Id,
                                                                   ClientId = client.ClientId,
                                                                   Title = client.Title,
                                                                   FirstName = client.FirstName,
                                                                   MiddleName = client.MiddleName,
                                                                   LastName = client.LastName,
                                                                   ProviderPosition = client.ProviderPosition,
                                                                   PhoneNo = client.PhoneNo,
                                                                   Email = client.Email,
                                                                   ContactMetod = client.ContactMetod,
                                                                   FullName = client.FirstName + " " + (!string.IsNullOrEmpty(client.MiddleName) ? client.MiddleName : null) + " " + client.LastName
                                                               }).FirstOrDefault();
                if (_clientDetails.ClientAccidentPrimaryContact == null) _clientDetails.ClientAccidentPrimaryContact = new LHSAPI.Application.Client.Models.ClientAccidentPrimaryContact();
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


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

namespace LHSAPI.Application.Client.Queries.GetAccidentIncidentInfo
{
    public class GetAccidentIncidentInfoHandler : IRequestHandler<GetAccidentIncidentInfoQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetAccidentIncidentInfoHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetAccidentIncidentInfoQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientAccidentIncidentDetails _clientDetails = new ClientAccidentIncidentDetails();

                _clientDetails.ClientIncidentDetails = (from accident in _dbContext.ClientIncidentDetails
                                                        where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                        select new LHSAPI.Application.Client.Models.ClientIncidentDetails
                                                        {
                                                            Id = accident.Id,
                                                            ClientId = accident.ClientId,
                                                            LocationId = accident.LocationId,
                                                            LocationType = accident.LocationType,
                                                            OtherLocation = accident.OtherLocation,
                                                            DateTime = accident.DateTime,
                                                            UnknowndateReason = accident.UnknowndateReason,
                                                            NdisProviderDate = accident.NdisProviderDate,
                                                            NdisProviderTime = accident.NdisProviderTime,
                                                            StartTimeString = accident.NdisProviderDate.Date.Add(accident.NdisProviderTime).ToString("hh:mm tt"),
                                                            AllegtionCircumstances = accident.AllegtionCircumstances,
                                                            IncidentAllegtion = accident.IncidentAllegtion,
                                                            LocationTypeName = _dbContext.StandardCode.Where(x => x.Value == accident.LocationType).Select(x => x.CodeDescription).FirstOrDefault(),
                                                            LocationName = _dbContext.StandardCode.Where(x => x.Value == accident.LocationId).Select(x => x.CodeDescription).FirstOrDefault(),
                                                            Address=accident.Address
                                                        }).FirstOrDefault();
                if (_clientDetails.ClientIncidentDetails == null) _clientDetails.ClientIncidentDetails = new LHSAPI.Application.Client.Models.ClientIncidentDetails();
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


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

namespace LHSAPI.Application.Client.Queries.GetIncidentDeclaration
{
    public class GetIncidentDeclarationHandler : IRequestHandler<GetIncidentDeclarationQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetIncidentDeclarationHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetIncidentDeclarationQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientDeclaration _clientDetails = new ClientDeclaration();

                _clientDetails.IncidentDeclaration = (from accident in _dbContext.IncidentDeclaration
                                                      where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                      select new LHSAPI.Application.Client.Models.IncidentDeclaration
                                                      {
                                                          Id = accident.Id,
                                                          ClientId = accident.ClientId,
                                                          Name = accident.Name,
                                                          PositionAtOrganisation = accident.PositionAtOrganisation,
                                                          Date = accident.Date,
                                                          IsDeclaration = accident.IsDeclaration,
                                                      }).FirstOrDefault();
                if (_clientDetails.IncidentDeclaration == null) _clientDetails.IncidentDeclaration = new LHSAPI.Application.Client.Models.IncidentDeclaration();
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

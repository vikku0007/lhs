
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

namespace LHSAPI.Application.Client.Queries.GetIncidentDocumentDetail
{
    public class GetIncidentDocumentDetailHandler : IRequestHandler<GetIncidentDocumentDetailQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        
        public GetIncidentDocumentDetailHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetIncidentDocumentDetailQuery request, CancellationToken cancellationToken)
        {
           
            ApiResponse response = new ApiResponse();
            try
            {
                ClientAttachment _clientDetails = new ClientAttachment();

                _clientDetails.IncidentDocumentDetailModel = (from accident in _dbContext.IncidentDocumentDetails
                                                              where accident.IsActive == true && accident.IsDeleted == false && accident.ClientId == request.Id && accident.ShiftId == request.ShiftId
                                                              select new LHSAPI.Application.Client.Models.IncidentDocumentDetailModel
                                                                  {
                                                                      Id = accident.Id,
                                                                      ClientId = accident.ClientId,
                                                                      DocumentName = accident.DocumentName,
                                                                      FileName=accident.FileName

                                                                  }).OrderByDescending(x => x.Id).ToList();
                
                if (_clientDetails.IncidentDocumentDetailModel == null) _clientDetails.IncidentDocumentDetailModel = new List<LHSAPI.Application.Client.Models.IncidentDocumentDetailModel>();
                
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

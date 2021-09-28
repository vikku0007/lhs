
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
using LHSAPI.Domain.Entities;
using LHSAPI.Common.Enums;

namespace LHSAPI.Application.Master.Queries.GetMandatoryTraining
{
    public class GetMandatoryTrainingQueryHandler : IRequestHandler<GetMandatoryTrainingQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetMandatoryTrainingQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
            // _logger = logger;
        }
       
        public async Task<ApiResponse> Handle(GetMandatoryTrainingQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var eventlist = (from mandatorytype in _dbContext.StandardCode
                                  where mandatorytype.CodeData == Common.Enums.ResponseEnums.StandardCode.MandatoryTraining.ToString() && mandatorytype.IsActive == true
                                  select new
                                  {
                                      mandatorytype.ID,
                                      mandatorytype.CodeDescription,
                                      mandatorytype.CodeData

                                  }).ToList();
                if (eventlist != null && eventlist.Any())
                {

                    response.SuccessWithOutMessage(eventlist.ToList());

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
       
    }
}











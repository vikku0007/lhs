
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

namespace LHSAPI.Application.Master.Queries.GetDocumentName
{
    public class GetDocumentNameQueryHandler : IRequestHandler<GetDocumentNameQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetDocumentNameQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetDocumentNameQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var servicelist = (from documentname in _dbContext.StandardCode
                                  where documentname.CodeData == Common.Enums.ResponseEnums.StandardCode.DocumentName.ToString() && documentname.IsActive == true
                                  select new
                                  {
                                      documentname.ID,
                                      documentname.CodeDescription,
                                      documentname.CodeData

                                  }).ToList();
                if (servicelist != null && servicelist.Any())
                {

                    response.SuccessWithOutMessage(servicelist.ToList());

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











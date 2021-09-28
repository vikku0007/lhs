
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

namespace LHSAPI.Application.Master.Queries.GetOptionalDocument
{
    public class GetOptionalDocumentHandler : IRequestHandler<GetOptionalDocumentQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetOptionalDocumentHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetOptionalDocumentQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var levellist = (from type in _dbContext.StandardCode
                                 where type.CodeData == Common.Enums.ResponseEnums.StandardCode.Optionaldocument.ToString() && type.IsActive == true
                                 select new
                                 {
                                     type.ID,
                                     type.CodeDescription,
                                     type.CodeData,
                                     Value = type.Value == 11 ? "Support planning" : type.Value == 12 ? "Behavior support/specialist reports" :
                                     type.Value == 13 ? "Individualized documents" : type.Value == 14 ? "Health planning" : type.Value == 15 ? "Health notes/ Hospital admission documents" :
                                     type.Value == 16 ? "Section 7 CHAPS" : type.Value == 17 ? "Treatment sheets/doctors forms" : type.Value == 18 ? "Incident reporting/ Complaint/ Feedback " : ""
                                 }).ToList();
                if (levellist != null && levellist.Any())
                {

                    response.SuccessWithOutMessage(levellist.ToList());

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











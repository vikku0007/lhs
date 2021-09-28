using LHSAPI.Application.Master.Queries.GetDepartment;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace LHSAPI.Application.Master.Queries.GetDocument
{
    public class GetDocumentQueryHandler: IRequestHandler<GetDocumentQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        //   readonly ILoggerManager _logger;
        public GetDocumentQueryHandler(LHSDbContext dbContext)
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
        public async Task<ApiResponse> Handle(GetDocumentQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                var deptlist = (from doc in _dbContext.StandardCode
                                where doc.CodeData == Common.Enums.ResponseEnums.StandardCode.DocumentType.ToString() && doc.IsActive == true && doc.IsDeleted == false
                                select new
                                {
                                    doc.ID,
                                    doc.CodeDescription,
                                    doc.CodeData

                                }).ToList();
                if (deptlist != null && deptlist.Any())
                {

                    response.SuccessWithOutMessage(deptlist.ToList());

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

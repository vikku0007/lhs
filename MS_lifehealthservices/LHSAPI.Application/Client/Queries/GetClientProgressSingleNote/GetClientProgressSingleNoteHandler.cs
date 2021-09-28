
using LHSAPI.Application.Client.Models;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LHSAPI.Common.Enums.ResponseEnums;

namespace LHSAPI.Application.Client.Queries.GetClientProgressSingleNote
{
    public class GetClientProgressSingleNoteQueryHandler : IRequestHandler<GetClientProgressSingleNoteQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        public GetClientProgressSingleNoteQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetClientProgressSingleNoteQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientPrimaryInfo _clientDetails = new ClientPrimaryInfo();

                //  var clientList = _dbContext.ClientPrimaryInfo.Where(x => x.IsDeleted == false && x.IsActive && x.Id == request.Id).FirstOrDefault();
                if (request.ClientId > 0)
                {
                    var ClientprimaryInfo = _dbContext.ClientProgressNotes.Where(x => x.IsDeleted == false && x.IsActive && x.ClientId == request.ClientId).FirstOrDefault();
                    
                    response.SuccessWithOutMessage(ClientprimaryInfo);
                }

                else
                {
                    response.Status = (int)Number.Zero;
                    response.Message = ResponseMessage.NOTFOUND;
                    response.StatusCode = HTTPStatusCode.NO_DATA_FOUND;
                }

            }
            catch (Exception ex)
            {
                response.Status = (int)Number.Zero;
                response.Message = ResponseMessage.Error;
                response.StatusCode = HTTPStatusCode.INTERNAL_SERVER_ERROR;
            }
            return response;
        }
        #endregion
    }
}

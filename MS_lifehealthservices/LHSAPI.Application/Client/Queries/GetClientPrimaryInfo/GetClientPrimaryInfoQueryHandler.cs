
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

namespace LHSAPI.Application.Client.Queries.GetClientPrimaryInfo
{
    public class GetClientPrimaryInfoQueryHandler : IRequestHandler<GetClientPrimaryInfoQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        public GetClientPrimaryInfoQueryHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        #region Client

        public async Task<ApiResponse> Handle(GetClientPrimaryInfoQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();
            try
            {
                ClientPrimaryInfo _clientDetails = new ClientPrimaryInfo();
                if (request.Id > 0)
                {
                    var ClientprimaryInfo = _dbContext.ClientPrimaryInfo.Where(x => x.IsDeleted == false  && x.Id == request.Id).ToList();
                    if (ClientprimaryInfo.Count > 0)
                    {
                        foreach (var item in ClientprimaryInfo)
                        {
                            ClientPrimaryInfo primaryInfo = new ClientPrimaryInfo
                            {
                                Id = item.Id,
                                ClientId = item.ClientId,
                                Salutation = item.Salutation,
                                FirstName = item.FirstName,
                                LastName = item.LastName,
                                MiddleName = item.MiddleName,
                                EmailId = item.EmailId,
                                DateOfBirth = item.DateOfBirth,
                                MaritalStatus = item.MaritalStatus,
                                MobileNo = item.MobileNo,
                                Address = item.Address,
                                Gender = item.Gender,
                                FullName = item.FirstName + " " + (!string.IsNullOrEmpty(item.MiddleName) ? item.MiddleName : null) + " " + item.LastName
                            };

                            _clientDetails = primaryInfo;
                        }
                    }
                    response = response.Success(_clientDetails);
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

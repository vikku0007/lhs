using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace LHSAPI.Application.Client.Commands.Update.UpdateClientFundingInfo
{
    public class UpdateClientFundingCommandHandler : IRequestHandler<UpdateClientFundingCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public UpdateClientFundingCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(UpdateClientFundingCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var _ClientFundingInfo = _context.ClientFunding.FirstOrDefault(x => x.Id == request.Id & x.IsActive == true && x.IsDeleted == false);
                    if (_ClientFundingInfo != null)
                    {

                        _ClientFundingInfo.ServiceType = (request.ServiceType);
                        _ClientFundingInfo.Ammount = Convert.ToDouble(request.Ammount);
                        _ClientFundingInfo.NoDays = request.NoDays;
                        _ClientFundingInfo.TotalAmount = request.TotalAmount;
                        _ClientFundingInfo.ClaimNumber = request.ClaimNumber;
                        _ClientFundingInfo.PaymentTerm = request.PaymentTerm;
                        _ClientFundingInfo.Payer = request.Payer;
                        _ClientFundingInfo.ReferenceNumber = request.ReferenceNumber;
                        _ClientFundingInfo.UpdateById = await _ISessionService.GetUserId();
                        _ClientFundingInfo.UpdatedDate = DateTime.Now;
                        _context.ClientFunding.Update(_ClientFundingInfo);
                        _context.SaveChanges();
                        response.Update(_ClientFundingInfo);
                    }
                    else
                    {
                        response.NotFound();
                    }
                }
                else
                {
                    response.ValidationError();
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

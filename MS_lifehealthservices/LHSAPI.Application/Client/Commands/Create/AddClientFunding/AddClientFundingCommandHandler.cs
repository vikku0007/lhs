
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;

using System.Linq;

using System.Threading;
using System.Threading.Tasks;


namespace LHSAPI.Application.Client.Commands.Create.AddClientFunding
{
    public class AddClientFundingCommandHandler : IRequestHandler<AddClientFundingCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddClientFundingCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddClientFundingCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {

                    //var ExistUser = _context.ClientFunding.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false);
                    //if (ExistUser == null)
                    //{
                        ClientFunding _ClientFunding = new ClientFunding();
                        _ClientFunding.ServiceType = (request.ServiceType);
                        _ClientFunding.Ammount = Convert.ToDouble(request.Ammount);
                        _ClientFunding.NoDays = request.NoDays;
                        _ClientFunding.TotalAmount = request.TotalAmount;
                        _ClientFunding.ClaimNumber = request.ClaimNumber;
                        _ClientFunding.PaymentTerm = request.PaymentTerm;
                        _ClientFunding.Payer = request.Payer;
                        _ClientFunding.ReferenceNumber = request.ReferenceNumber;
                        _ClientFunding.CreatedById = await _ISessionService.GetUserId();
                        _ClientFunding.CreatedDate = DateTime.Now;
                        _ClientFunding.IsActive = true;
                        _ClientFunding.IsDeleted = false;
                        _ClientFunding.ClientId = request.ClientId;
                        await _context.ClientFunding.AddAsync(_ClientFunding);
                        _context.SaveChanges();
                        response.Update(_ClientFunding);
                    //}
                    //else
                    //{
                    //    response.AlreadyExist();
                    //}
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

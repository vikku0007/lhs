
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;

using System.Linq;

using System.Threading;
using System.Threading.Tasks;


namespace LHSAPI.Application.Client.Commands.Create.AddClientFundingInfo
{
    public class AddClientFundingInfoCommandHandler : IRequestHandler<AddClientFundingInfoCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddClientFundingInfoCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddClientFundingInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {

                    var ExistUser = _context.ClientFundingInfo.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false
                    && x.Id == request.Id && request.Id != 0);
                    if (ExistUser == null)
                    {
                        ClientFundingInfo _ClientFunding = new ClientFundingInfo();
                        _ClientFunding.Other = request.Other;
                        _ClientFunding.RefNumber = request.RefNumber;
                        _ClientFunding.FundType = request.FundType;
                        _ClientFunding.StartDate = request.StartDate;
                        _ClientFunding.EndDate = request.EndDate;
                        _ClientFunding.ClaimNumber = request.ClaimNumber;
                        _ClientFunding.CreatedById = await _ISessionService.GetUserId();
                        _ClientFunding.CreatedDate = DateTime.Now;
                        _ClientFunding.IsActive = true;
                        _ClientFunding.IsDeleted = false;
                        _ClientFunding.ClientId = request.ClientId;
                        _ClientFunding.Amount = Convert.ToDouble(request.Amount);
                        await _context.ClientFundingInfo.AddAsync(_ClientFunding);
                        _context.SaveChanges();
                        response.Success(_ClientFunding);
                    }
                    else
                    {
                        ExistUser.Other = request.Other;
                        ExistUser.RefNumber = request.RefNumber;
                        ExistUser.FundType = request.FundType;
                        ExistUser.StartDate = request.StartDate;
                        ExistUser.EndDate = request.EndDate;
                        ExistUser.Amount = Convert.ToDouble(request.Amount);
                        ExistUser.ClaimNumber = request.ClaimNumber;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.ClientFundingInfo.Update(ExistUser);
                        await _context.SaveChangesAsync();
                        response.Update(ExistUser);
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

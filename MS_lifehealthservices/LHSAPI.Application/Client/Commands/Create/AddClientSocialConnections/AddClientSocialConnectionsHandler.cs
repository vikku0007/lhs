
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;

using System.Linq;

using System.Threading;
using System.Threading.Tasks;


namespace LHSAPI.Application.Client.Commands.Create.AddClientSocialConnections
{
    public class AddClientSocialConnectionsHandler : IRequestHandler<AddClientSocialConnectionsCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddClientSocialConnectionsHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddClientSocialConnectionsCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {

                    var ExistUser = _context.ClientSocialConnections.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        ClientSocialConnections _Client = new ClientSocialConnections();
                        _Client.SocialConnection = request.SocialConnection;
                        _Client.CreatedById = await _ISessionService.GetUserId();
                        _Client.CreatedDate = DateTime.Now;
                        _Client.IsActive = true;
                        _Client.IsDeleted = false;
                        _Client.ClientId = request.ClientId;
                        await _context.ClientSocialConnections.AddAsync(_Client);
                        _context.SaveChanges();
                        response.Success(_Client);

                    }
                    else
                    {
                        ExistUser.SocialConnection = request.SocialConnection;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.ClientSocialConnections.Update(ExistUser);
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

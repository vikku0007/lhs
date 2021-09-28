
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;

using System.Linq;

using System.Threading;
using System.Threading.Tasks;


namespace LHSAPI.Application.Client.Commands.Create.AddClientCultureNeed
{
    public class AddClientCultureNeedHandler : IRequestHandler<AddClientCultureNeedCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddClientCultureNeedHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;

        }

        public async Task<ApiResponse> Handle(AddClientCultureNeedCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {

                    var ExistUser = _context.ClientCultureNeed.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        ClientCultureNeed _ClientCultureNeed = new ClientCultureNeed();
                        _ClientCultureNeed.CultureNeed = request.CultureNeed;
                        _ClientCultureNeed.CreatedById = await _ISessionService.GetUserId();
                        _ClientCultureNeed.CreatedDate = DateTime.Now;
                        _ClientCultureNeed.IsActive = true;
                        _ClientCultureNeed.IsDeleted = false;
                        _ClientCultureNeed.ClientId = request.ClientId;
                        await _context.ClientCultureNeed.AddAsync(_ClientCultureNeed);
                        _context.SaveChanges();
                        response.Success(_ClientCultureNeed);

                    }
                    else
                    {
                        ExistUser.CultureNeed = request.CultureNeed;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.ClientCultureNeed.Update(ExistUser);
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

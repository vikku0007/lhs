
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;

using System.Linq;

using System.Threading;
using System.Threading.Tasks;


namespace LHSAPI.Application.Client.Commands.Create.AddClientPreferences
{
    public class AddClientPreferencesHandler : IRequestHandler<AddClientPreferencesCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddClientPreferencesHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddClientPreferencesCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {

                    var ExistUser = _context.ClientPersonalPreferences.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        ClientPersonalPreferences _Client = new ClientPersonalPreferences();
                        _Client.Interest = request.Interest;
                        _Client.ClientImportance = request.ClientImportance;
                        _Client.Charecteristics = request.Charecteristics;
                        _Client.FearsandAnxities = request.FearsandAnxities;
                        _Client.CreatedById = await _ISessionService.GetUserId();
                        _Client.CreatedDate = DateTime.Now;
                        _Client.IsActive = true;
                        _Client.IsDeleted = false;
                        _Client.ClientId = request.ClientId;
                        await _context.ClientPersonalPreferences.AddAsync(_Client);
                        _context.SaveChanges();
                        response.Success(_Client);

                    }
                    else
                    {
                        ExistUser.Interest = request.Interest;
                        ExistUser.ClientImportance = request.ClientImportance;
                        ExistUser.Charecteristics = request.Charecteristics;
                        ExistUser.FearsandAnxities = request.FearsandAnxities;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.ClientPersonalPreferences.Update(ExistUser);
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

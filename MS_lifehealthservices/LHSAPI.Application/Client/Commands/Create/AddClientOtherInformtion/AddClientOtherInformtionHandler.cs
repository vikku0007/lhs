
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;

using System.Linq;

using System.Threading;
using System.Threading.Tasks;


namespace LHSAPI.Application.Client.Commands.Create.AddClientOtherInformtion
{
    public class AddClientOtherInformtionHandler : IRequestHandler<AddClientOtherInformtionCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddClientOtherInformtionHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddClientOtherInformtionCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {

                    var ExistUser = _context.ClientOtherInformtion.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        ClientOtherInformtion _Client = new ClientOtherInformtion();
                        _Client.OtherInformation = request.OtherInformation;
                        _Client.CreatedById = await _ISessionService.GetUserId();
                        _Client.CreatedDate = DateTime.Now;
                        _Client.IsActive = true;
                        _Client.IsDeleted = false;
                        _Client.ClientId = request.ClientId;
                        await _context.ClientOtherInformtion.AddAsync(_Client);
                        _context.SaveChanges();
                        response.Success(_Client);

                    }
                    else
                    {
                        ExistUser.OtherInformation = request.OtherInformation;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.ClientOtherInformtion.Update(ExistUser);
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


using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;

using System.Linq;

using System.Threading;
using System.Threading.Tasks;


namespace LHSAPI.Application.Client.Commands.Create.AddClientAdditionalNotes
{
    public class AddClientAdditionalNotesCommandHandler : IRequestHandler<AddClientAdditionalNotesCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;

        public AddClientAdditionalNotesCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddClientAdditionalNotesCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {

                    var ExistUser = _context.ClientAdditionalNotes.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsActive == false);
                    if (ExistUser == null)
                    {
                        ClientAdditionalNotes _ClientAdditionalNotes = new ClientAdditionalNotes();
                        _ClientAdditionalNotes.PrivateNote = request.PrivateNote;
                        _ClientAdditionalNotes.PublicNote = request.PublicNote;
                        _ClientAdditionalNotes.CreatedById = await _ISessionService.GetUserId();
                        _ClientAdditionalNotes.CreatedDate = DateTime.Now;
                        _ClientAdditionalNotes.IsActive = true;
                        _ClientAdditionalNotes.IsDeleted = false;
                        _ClientAdditionalNotes.ClientId = request.ClientId;
                        await _context.ClientAdditionalNotes.AddAsync(_ClientAdditionalNotes);
                        _context.SaveChanges();
                        response.Success(_ClientAdditionalNotes);

                    }
                    else
                    {
                        ExistUser.PublicNote = request.PublicNote;
                        ExistUser.PrivateNote = request.PrivateNote;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.ClientAdditionalNotes.Update(ExistUser);
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

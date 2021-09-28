
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace LHSAPI.Application.Client.Commands.Create.AddClientBoardingNotes
{
    public class AddClientBoardingNotesCommandHandler : IRequestHandler<AddClientBoardingNotesCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddClientBoardingNotesCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddClientBoardingNotesCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {

                    var ExistUser = _context.ClientBoadingNotes.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true);
                    if (ExistUser == null)
                    {
                        ClientBoadingNotes _ClientBoardingNotes = new ClientBoadingNotes();
                        _ClientBoardingNotes.CareNote = request.CareNote;
                        _ClientBoardingNotes.CreatedById = await _ISessionService.GetUserId();
                        _ClientBoardingNotes.CreatedDate = DateTime.Now;
                        _ClientBoardingNotes.IsActive = true;
                        _ClientBoardingNotes.IsDeleted = false;
                        _ClientBoardingNotes.ClientId = request.ClientId;
                        await _context.ClientBoadingNotes.AddAsync(_ClientBoardingNotes);
                        _context.SaveChanges();
                        response.Success(_ClientBoardingNotes);

                    }
                    else
                    {
                        ExistUser.CareNote = request.CareNote;
                        ExistUser.UpdateById = 1;
                        ExistUser.UpdatedDate = DateTime.Now;
                        ExistUser.IsActive = true;
                        _context.ClientBoadingNotes.Update(ExistUser);
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

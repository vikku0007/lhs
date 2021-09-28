using LHSAPI.Application.Interface;
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

namespace LHSAPI.Application.Client.Commands.Delete.DeleteClientProgressNotesItem
{

    public class DeleteClientProgressNotesItemCommandHandler : IRequestHandler<DeleteClientProgressNotesItemCommand, ApiResponse>
    {

        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public DeleteClientProgressNotesItemCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(DeleteClientProgressNotesItemCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var Existclient = _context.ProgressNotesList.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                    if (Existclient == null)
                    {
                        response.NotFound();
                    }
                    else
                    {

                        Existclient.IsDeleted = true;
                        Existclient.IsActive = false;
                        Existclient.DeletedDate = DateTime.UtcNow;
                        Existclient.DeletedById = await _ISessionService.GetUserId();
                        _context.ProgressNotesList.Update(Existclient);
                        await _context.SaveChangesAsync();
                        response.Delete(Existclient);

                    }
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

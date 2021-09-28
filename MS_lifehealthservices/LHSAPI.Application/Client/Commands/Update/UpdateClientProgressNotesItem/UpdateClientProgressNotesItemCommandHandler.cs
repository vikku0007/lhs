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


namespace LHSAPI.Application.Client.Commands.Update.UpdateClientProgressNotesItem
{
    public class UpdateClientProgressNotesItemCommandHandler : IRequestHandler<UpdateClientProgressNotesItemCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public UpdateClientProgressNotesItemCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(UpdateClientProgressNotesItemCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var _ProgressNotesList = _context.ProgressNotesList.FirstOrDefault(x => x.Id == request.Id & x.IsActive == true && x.IsDeleted == false);
                    if (_ProgressNotesList != null)
                    {

                        _ProgressNotesList.ClientId = request.ClientId;
                        _ProgressNotesList.ClientProgressNoteId = request.ClientProgressNoteId;
                        _ProgressNotesList.Date = request.Date;
                        _ProgressNotesList.Note9AMTo11AM = request.Note9AMTo11AM;
                        _ProgressNotesList.Note11AMTo1PM = request.Note11AMTo1PM;
                        _ProgressNotesList.Note1PMTo15PM = request.Note1PMTo15PM;
                        _ProgressNotesList.Note15PMTo17PM = request.Note15PMTo17PM;
                        _ProgressNotesList.Note17PMTo19PM = request.Note17PMTo19PM;
                        _ProgressNotesList.Note19PMTo21PM = request.Note19PMTo21PM;
                        _ProgressNotesList.Note21PMTo23PM = request.Note21PMTo23PM;
                        _ProgressNotesList.Note23PMTo1AM = request.Note23PMTo1AM;
                        _ProgressNotesList.Note1AMTo3AM = request.Note1AMTo3AM;
                        _ProgressNotesList.Note3AMTo5AM = request.Note3AMTo5AM;
                        _ProgressNotesList.Note5AMTo7AM = request.Note5AMTo7AM;
                        _ProgressNotesList.Note7AMTo9AM = request.Note7AMTo9AM;
                        _ProgressNotesList.Summary = request.Summary;
                        _ProgressNotesList.OtherInfo = request.OtherInfo;
                        _ProgressNotesList.UpdateById = await _ISessionService.GetUserId();
                        _ProgressNotesList.UpdatedDate = DateTime.Now;
                        _ProgressNotesList.IsActive = true;
                        _ProgressNotesList.IsDeleted = false;
                        _context.ProgressNotesList.Update(_ProgressNotesList);
                        _context.SaveChanges();
                        response.Update(_ProgressNotesList);
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

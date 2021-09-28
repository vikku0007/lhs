
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Domain.Entities;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;

using System.Linq;

using System.Threading;
using System.Threading.Tasks;


namespace LHSAPI.Application.Client.Commands.Create.AddClientProgressNotes
{
    public class AddClientProgressNotesCommandHandler : IRequestHandler<AddClientProgressNotesCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddClientProgressNotesCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddClientProgressNotesCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0)
                {

                    var ExistUser = _context.ClientProgressNotes.FirstOrDefault(x => x.ClientId == request.ClientId && x.PatientName == request.PatientName && x.IsActive == true && x.IsDeleted == false);
                    if (ExistUser == null)
                    {
                        ClientProgressNotes _ClientProgressNotes = new ClientProgressNotes();
                        _ClientProgressNotes.ClientId = request.ClientId;
                        _ClientProgressNotes.PatientName = request.PatientName;
                        _ClientProgressNotes.DateOfBirth = request.DateOfBirth;
                        _ClientProgressNotes.MedicalRecordNo = request.MedicalRecordNo;
                        _ClientProgressNotes.CreatedById = await _ISessionService.GetUserId();
                        _ClientProgressNotes.CreatedDate = DateTime.Now;
                        _ClientProgressNotes.IsActive = true;
                        _ClientProgressNotes.IsDeleted = false;
                        await _context.ClientProgressNotes.AddAsync(_ClientProgressNotes);
                        _context.SaveChanges();
                        response.Success(_ClientProgressNotes);
                    }
                    else
                    {
                        ExistUser.PatientName = request.PatientName;
                        ExistUser.DateOfBirth = request.DateOfBirth;
                        ExistUser.MedicalRecordNo = request.MedicalRecordNo;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        _context.ClientProgressNotes.Update(ExistUser);
                        _context.SaveChanges();
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

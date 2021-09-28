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

namespace LHSAPI.Application.Client.Commands.Delete.DeleteClientListing
{
    public class DeleteClientListingCommandHandler : IRequestHandler<DeleteClientListingCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public DeleteClientListingCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(DeleteClientListingCommand request, CancellationToken cancellationToken)
        {

            ApiResponse response = new ApiResponse();
            try
            {
                if (request.Id > 0)
                {

                    var ExistEmp = _context.ClientPrimaryInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false);
                    if (ExistEmp == null)
                    {
                        response.NotFound();
                    }
                    else
                    {
                        var AccidentEmp = _context.ClientAccidentIncidentInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var AdditionalEmp = _context.ClientAdditionalNotes.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var BoardingEmp = _context.ClientBoadingNotes.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var FundingEmp = _context.ClientFunding.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var FundingInfoEmp = _context.ClientFundingInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var ComplianceEmp = _context.ClientCompliancesDetails.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var MedicalEmp = _context.ClientMedicalHistory.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var PicEmp = _context.ClientPicInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var CarerEmp = _context.ClientPrimaryCareInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var NotesEmp = _context.ClientProgressNotes.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var ShiftEmp = _context.ClientShiftInfo.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);
                        var NotesListEmp = _context.ProgressNotesList.FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false && x.IsActive == true);

                        if (AccidentEmp != null)
                        {
                            AccidentEmp.IsDeleted = true;
                            AccidentEmp.IsActive = false;
                            AccidentEmp.DeletedDate = DateTime.UtcNow;
                            AccidentEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.ClientAccidentIncidentInfo.Update(AccidentEmp);
                        }
                        if (AdditionalEmp != null)
                        {
                            AdditionalEmp.IsDeleted = true;
                            AdditionalEmp.IsActive = false;
                            AdditionalEmp.DeletedDate = DateTime.UtcNow;
                            AdditionalEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.ClientAdditionalNotes.Update(AdditionalEmp);
                        }

                        if (BoardingEmp != null)
                        {
                            BoardingEmp.IsDeleted = true;
                            BoardingEmp.IsActive = false;
                            BoardingEmp.DeletedDate = DateTime.UtcNow;
                            BoardingEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.ClientBoadingNotes.Update(BoardingEmp);
                        }

                        if (FundingEmp != null)
                        {
                            FundingEmp.IsDeleted = true;
                            FundingEmp.IsActive = false;
                            FundingEmp.DeletedDate = DateTime.UtcNow;
                            FundingEmp.DeletedById = await _ISessionService.GetUserId();
                            _context.ClientFunding.Update(FundingEmp);
                        }
                        if (FundingInfoEmp != null)
                        {
                            FundingInfoEmp.IsActive = false;
                            FundingInfoEmp.DeletedDate = DateTime.UtcNow;
                            FundingInfoEmp.DeletedById = await _ISessionService.GetUserId();
                            FundingInfoEmp.IsDeleted = true;
                            _context.ClientFundingInfo.Update(FundingInfoEmp);
                        }

                        if (ComplianceEmp != null)
                        {
                            ComplianceEmp.IsActive = false;
                            ComplianceEmp.DeletedDate = DateTime.UtcNow;
                            ComplianceEmp.DeletedById = await _ISessionService.GetUserId();
                            ComplianceEmp.IsDeleted = true;
                            _context.ClientCompliancesDetails.Update(ComplianceEmp);
                        }

                        if (MedicalEmp != null)
                        {
                            MedicalEmp.IsActive = false;
                            MedicalEmp.DeletedDate = DateTime.UtcNow;
                            MedicalEmp.DeletedById = await _ISessionService.GetUserId();
                            MedicalEmp.IsDeleted = true;
                            _context.ClientMedicalHistory.Update(MedicalEmp);
                        }

                        if (CarerEmp != null)
                        {
                            CarerEmp.IsActive = false;
                            CarerEmp.DeletedDate = DateTime.UtcNow;
                            CarerEmp.DeletedById = await _ISessionService.GetUserId();
                            CarerEmp.IsDeleted = true;
                            _context.ClientPrimaryCareInfo.Update(CarerEmp);
                        }

                        if (NotesEmp != null)
                        {
                            NotesEmp.IsActive = false;
                            NotesEmp.DeletedDate = DateTime.UtcNow;
                            NotesEmp.DeletedById = await _ISessionService.GetUserId();
                            NotesEmp.IsDeleted = true;
                            _context.ClientProgressNotes.Update(NotesEmp);
                        }

                        if (ShiftEmp != null)
                        {
                            ShiftEmp.IsActive = false;
                            ShiftEmp.DeletedDate = DateTime.UtcNow;
                            ShiftEmp.DeletedById = await _ISessionService.GetUserId();
                            ShiftEmp.IsDeleted = true;
                            _context.ClientShiftInfo.Update(ShiftEmp);
                        }
                        if (NotesListEmp != null)
                        {
                            NotesListEmp.IsActive = false;
                            NotesListEmp.DeletedDate = DateTime.UtcNow;
                            NotesListEmp.DeletedById = await _ISessionService.GetUserId();
                            NotesListEmp.IsDeleted = true;
                            _context.ProgressNotesList.Update(NotesListEmp);
                        }
                        ExistEmp.IsDeleted = true;
                        ExistEmp.DeletedDate = DateTime.UtcNow;
                        ExistEmp.DeletedById = await _ISessionService.GetUserId();
                        _context.ClientPrimaryInfo.Update(ExistEmp);
                        await _context.SaveChangesAsync();
                        response.Delete(ExistEmp);

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

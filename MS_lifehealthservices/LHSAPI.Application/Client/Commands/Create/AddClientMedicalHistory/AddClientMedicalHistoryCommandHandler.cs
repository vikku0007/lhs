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

namespace LHSAPI.Application.Client.Commands.Create.AddClientMedicalHistory
{
    public class AddClientMedicalHistoryCommandHandler: IRequestHandler<AddClientMedicalHistoryCommand, ApiResponse>
    {
        private readonly LHSDbContext _context;
        private readonly ISessionService _ISessionService;
        public AddClientMedicalHistoryCommandHandler(LHSDbContext context, ISessionService ISessionService)
        {
            _context = context;
            _ISessionService = ISessionService;
        }

        public async Task<ApiResponse> Handle(AddClientMedicalHistoryCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                if (request.ClientId > 0 && request.ShiftId > 0)
                {
                   
                   
                    var  ExistUser = _context.ClientMedicalHistory.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false && x.ShiftId == request.ShiftId);
                   
                    if (ExistUser == null)
                    {
                        ClientMedicalHistory _clientMedicalHistory = new ClientMedicalHistory();
                        _clientMedicalHistory.ClientId = request.ClientId;
                        _clientMedicalHistory.ShiftId = request.ShiftId;
                        _clientMedicalHistory.EmployeeId = request.EmployeeId;
                        _clientMedicalHistory.Name = request.Name;
                        _clientMedicalHistory.MobileNo = request.MobileNo;
                        _clientMedicalHistory.Gender = request.Gender;
                        _clientMedicalHistory.CheckCondition = request.CheckCondition;
                        _clientMedicalHistory.CheckSymptoms = request.CheckSymptoms;
                        _clientMedicalHistory.IsTakingMedication = request.IsTakingMedication;
                        _clientMedicalHistory.IsMedicationAllergy = request.IsMedicationAllergy;
                        _clientMedicalHistory.IsTakingTobacco = request.IsTakingTobacco;
                        _clientMedicalHistory.IsTakingIllegalDrug = request.IsTakingIllegalDrug;
                        _clientMedicalHistory.TakingAlcohol = request.TakingAlcohol;
                        _clientMedicalHistory.MedicationDetail = request.MedicationDetail;
                        _clientMedicalHistory.OtherCondition = request.OtherCondition;
                        _clientMedicalHistory.OtherSymptoms = request.OtherSymptoms;
                        _clientMedicalHistory.IsDeleted = false;
                        _clientMedicalHistory.IsActive = true;
                        _clientMedicalHistory.CreatedById = await _ISessionService.GetUserId();
                        _clientMedicalHistory.CreatedDate = DateTime.Now;
                        await _context.ClientMedicalHistory.AddAsync(_clientMedicalHistory);
                        _context.SaveChanges();
                        response.Success(_clientMedicalHistory);
                    }
                    else
                    {

                        ExistUser.ClientId = request.ClientId;
                        ExistUser.Name = request.Name;
                        ExistUser.MobileNo = request.MobileNo;
                        ExistUser.Gender = request.Gender;
                        ExistUser.CheckCondition = request.CheckCondition;
                        ExistUser.CheckSymptoms = request.CheckSymptoms;
                        ExistUser.IsTakingMedication = request.IsTakingMedication;
                        ExistUser.IsMedicationAllergy = request.IsMedicationAllergy;
                        ExistUser.IsTakingTobacco = request.IsTakingTobacco;
                        ExistUser.IsTakingIllegalDrug = request.IsTakingIllegalDrug;
                        ExistUser.TakingAlcohol = request.TakingAlcohol;
                        ExistUser.MedicationDetail = request.MedicationDetail;
                        ExistUser.OtherCondition = request.OtherCondition;
                        ExistUser.OtherSymptoms = request.OtherSymptoms;
                        ExistUser.IsDeleted = false;
                        ExistUser.IsActive = true;
                        ExistUser.UpdateById = await _ISessionService.GetUserId();
                        ExistUser.UpdatedDate = DateTime.Now;
                        _context.ClientMedicalHistory.Update(ExistUser);
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

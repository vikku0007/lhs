using LHSAPI.Application.Client.Models;
using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LHSAPI.Application.Client.Queries.GetClientMedicalHistoryInfo
{
    public class GetClientMedicalHistoryQueryHandler : IRequestHandler<GetClientMedicalHistoryQuery, ApiResponse>
    {
        private readonly LHSDbContext _context;
        //   readonly ILoggerManager _logger;
        public GetClientMedicalHistoryQueryHandler(LHSDbContext dbContext)
        {
            _context = dbContext;
            // _logger = logger;
        }

        public async Task<ApiResponse> Handle(GetClientMedicalHistoryQuery request, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            ApiResponse response = new ApiResponse();

            try
            {
                if (request.ClientId > 0 && request.ShiftId > 0)
                {
                   var  ExistUser = _context.ClientMedicalHistory.FirstOrDefault(x => x.ClientId == request.ClientId && x.IsActive == true && x.IsDeleted == false && x.ShiftId == request.ShiftId);
                  
                    if (ExistUser != null)
                    {
                        ClientMedicalHistory _clientMedicalHistory = new ClientMedicalHistory();
                        _clientMedicalHistory.ClientId = ExistUser.ClientId;
                        // _clientMedicalHistory.Name = ExistUser.Name;
                        _clientMedicalHistory.MobileNo = ExistUser.MobileNo;
                        _clientMedicalHistory.Gender = ExistUser.Gender;
                        _clientMedicalHistory.CheckCondition = ExistUser.CheckCondition;
                        _clientMedicalHistory.CheckSymptoms = ExistUser.CheckSymptoms;
                        _clientMedicalHistory.IsTakingMedication = ExistUser.IsTakingMedication;
                        _clientMedicalHistory.IsMedicationAllergy = ExistUser.IsMedicationAllergy;
                        _clientMedicalHistory.IsTakingTobacco = ExistUser.IsTakingTobacco;
                        _clientMedicalHistory.IsTakingIllegalDrug = ExistUser.IsTakingIllegalDrug;
                        _clientMedicalHistory.TakingAlcohol = ExistUser.TakingAlcohol;                                                
                        _clientMedicalHistory.MedicationDetail = ExistUser.MedicationDetail;                                                
                        _clientMedicalHistory.OtherCondition = ExistUser.OtherCondition;                                                
                        _clientMedicalHistory.OtherSymptoms = ExistUser.OtherSymptoms;                                                
                        response.Success(_clientMedicalHistory);
                    }
                    else
                    {
                        response.NotFound();
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

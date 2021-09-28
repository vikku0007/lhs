using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddClientMedicalHistory
{
    public class AddClientMedicalHistoryCommand: IRequest<ApiResponse>
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public int Gender { get; set; }
        public int CheckCondition { get; set; }
        public int CheckSymptoms { get; set; }
        public bool IsTakingMedication { get; set; }
        public bool IsMedicationAllergy { get; set; }
        public bool IsTakingTobacco { get; set; }
        public bool IsTakingIllegalDrug { get; set; }
        public string TakingAlcohol { get; set; }
        public string MedicationDetail { get; set; }
        public string OtherCondition { get; set; }
        public string OtherSymptoms { get; set; }
        public int? ShiftId { get; set; }
        public int? EmployeeId { get; set; }
    }

}

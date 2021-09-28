using System;
using System.Collections.Generic;
using System.Text;


namespace LHSAPI.Application.Client.Models
{
    public class ClientMedicalHistory
    {

        public int Id { get; set; }

        public int ClientId { get; set; }

        public int Name { get; set; }

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

    }

}

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Domain.Entities
{
  public class ClientMedicalHistory : BaseEntity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

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

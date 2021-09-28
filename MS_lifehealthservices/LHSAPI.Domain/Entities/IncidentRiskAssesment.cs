using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LHSAPI.Domain.Entities
{
   public class IncidentRiskAssesment:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ClientId { get; set; }

        public int IsRiskAssesment { get; set; }

        public DateTime? RiskAssesmentDate { get; set; }

        public string RiskDetails { get; set; }

        public string NoRiskAssesmentInfo { get; set; }

        public string InProgressRisk { get; set; }

        public string TobeFinished { get; set; }
        public int? ShiftId { get; set; }
        public int? EmployeeId { get; set; }
    }
}

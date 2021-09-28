using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LHSAPI.Domain.Entities
{
  public class DayReportVisitor : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        public TimeSpan? Time { get; set; }

        public string VisitorName { get; set; }

        public string VisitReason { get; set; }

        public int ShiftId { get; set; }
    }
}

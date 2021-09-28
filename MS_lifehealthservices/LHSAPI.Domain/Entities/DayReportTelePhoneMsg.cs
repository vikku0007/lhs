using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LHSAPI.Domain.Entities
{
   public class DayReportTelePhoneMsg : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
      
        public TimeSpan? Time { get; set; }

        public string Caller { get; set; }

        public string MessageFor { get; set; }

        public string Message { get; set; }

        public string Signature { get; set; }

        public int ShiftId { get; set; }
    }
}

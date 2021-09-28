using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Domain.Entities
{
   public class DayReportAppointments : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public TimeSpan? Time { get; set; }

        public int ClientId { get; set; }

        public string Details { get; set; }

        public string GeneralInformation { get; set; }

        public string NightReport { get; set; }

        public int ShiftId { get; set; }

    }
}

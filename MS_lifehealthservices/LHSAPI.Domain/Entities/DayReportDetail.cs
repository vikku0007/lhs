using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LHSAPI.Domain.Entities
{
   public class DayReportDetail : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string HouseAddress { get; set; }

        public DateTime? Date { get; set; }

        public int ShiftId { get; set; }
    }
}

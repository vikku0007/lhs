using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LHSAPI.Domain.Entities
{
  public class DayReportFoodIntake : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       
        public string ResidentName { get; set; }

        public DateTime? Date { get; set; }

        public string DailyFoodIntake { get; set; }
        public string Signature { get; set; }

        public int ShiftId { get; set; }
        public string StaffName { get; set; }

        public string Breakfast { get; set; }

        public string Lunch { get; set; }

        public string Dinner { get; set; }

        public string Snacks { get; set; }

        public string fluids { get; set; }
    }
}

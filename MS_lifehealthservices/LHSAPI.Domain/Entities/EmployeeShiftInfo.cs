using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class EmployeeShiftInfo : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ShiftId { get; set; }

        public int EmployeeId { get; set; }
        public bool IsSleepOver { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsRejected { get; set; }
        public string Remark { get; set; }
        public bool IsActiveNight { get; set; }
        public bool IsHoliday { get; set; }
        public int? StatusId { get; set; }
        //public double TotalDuration { get; set; }

    }
}

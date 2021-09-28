using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class ShiftInfo : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Description { get; set; }

        public int ClientCount { get; set; }

        public int EmployeeCount { get; set; }

        public int? LocationId { get; set; }

        public string OtherLocation { get; set; }

        public bool IsPublished { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
        public DateTime StartUtcDate { get; set; }
        public DateTime EndUtcDate { get; set; }

        public bool Reminder { get; set; }

        public double Duration { get; set; }
        public int LocationType { get; set; }
        public string ShiftRepeatType { get; set; }
        public string Remark { get; set; }
    }
}

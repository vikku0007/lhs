using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class EmployeeJobProfile : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string JobDesc { get; set; }

        public int? DepartmentId { get; set; }
        public int? ReportingToId { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public int? Source { get; set; }
        public int? WorkingHoursWeekly { get; set; }

        public int? DistanceTravel { get; set; }

        
    }

}

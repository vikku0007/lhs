using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class EmployeeAvailabilityDetails : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public bool? WorksMon { get; set; }
         
        public string AvailabilityDay { get; set; }
    }

}

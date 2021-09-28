using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class EmployeeAccidentInfo : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DateTime AccidentDate { get; set; }

        public int? EventType { get; set; }

        public int? LocationId { get; set; }

        public int RaisedBy { get; set; }

        public int ReportedTo { get; set; }

        public string BriefDescription { get; set; }

        public string DetailedDescription { get; set; }
        public string OtherLocation { get; set; }
        public string ActionTaken { get; set; }

        public int? LocationType { get; set; }
        public TimeSpan IncidentTime { get; set; }
    }

}

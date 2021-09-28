using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class ClientAccidentIncidentInfo:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public int DepartmentId { get; set; }

        public DateTime? AccidentDate { get; set; }

        public int? IncidentType { get; set; }

        public int? LocationId { get; set; }

        public int ReportedBy { get; set; }

        public string PhoneNo { get; set; }
        public bool PoliceNotified { get; set; }

        public string IncidentCause { get; set; }

        public string FollowUp { get; set; }
        public string OtherLocation { get; set; }

        public int? LocationType { get; set; }

    }
}

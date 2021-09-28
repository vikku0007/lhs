using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LHSAPI.Domain.Entities
{
   public class ClientIncidentDetails : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ClientId { get; set; }

        public int LocationId { get; set; }

        public int? LocationType { get; set; }

        public string OtherLocation { get; set; }

        public DateTime? DateTime { get; set; }

        public string UnknowndateReason { get; set; }

        public TimeSpan NdisProviderTime { get; set; }

        public DateTime NdisProviderDate { get; set; }

        public string IncidentAllegtion { get; set; }

        public string AllegtionCircumstances { get; set; }
        public string Address { get; set; }
        public int? ShiftId { get; set; }
        public int? EmployeeId { get; set; }
    }
}

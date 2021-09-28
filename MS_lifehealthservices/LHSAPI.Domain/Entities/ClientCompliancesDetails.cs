using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
   public class ClientCompliancesDetails:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ClientId { get; set; }
        public int ShiftId { get; set; }

        public int DocumentName { get; set; }

        public int? DocumentType { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string Description { get; set; }

        public bool? HasExpiry { get; set; }

        public bool? Alert { get; set; }
        public string FileName { get; set; }

    }
}

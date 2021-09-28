using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LHSAPI.Domain.Entities
{
    public class EmployeeOtherComplianceDetails : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OtherDocumentName { get; set; }

       // public int? OtherDocumentType { get; set; }

        public DateTime? OtherIssueDate { get; set; }

        public DateTime? OtherExpiryDate { get; set; }

        public string OtherDescription { get; set; }

        public bool? OtherHasExpiry { get; set; }

        public bool? OtherAlert { get; set; }

        public int EmployeeId { get; set; }
        public string OtherFileName { get; set; }

    }

}

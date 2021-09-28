using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Domain.Entities
{
    public class ClientFunding : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ClientId { get; set; }

        public double Ammount { get; set; }

        public int? NoDays { get; set; }

        public int? ServiceType { get; set; }

        public double TotalAmount { get; set; }
        public int? ClaimNumber { get; set; }
        public int? PaymentTerm { get; set; }
        public int? Payer { get; set; }
        public int? ReferenceNumber { get; set; }

    }

}

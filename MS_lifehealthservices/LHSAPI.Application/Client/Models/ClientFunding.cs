using System;
using System.Collections.Generic;
using System.Text;


namespace LHSAPI.Application.Client.Models
{
    public class ClientFunding
    {

        public int Id { get; set; }

        public int ClientId { get; set; }

        public int ServiceType { get; set; }
        public string ServiceTypeName { get; set; }

        public double Ammount { get; set; }

        public int? NoDays { get; set; }

        public double TotalAmount { get; set; }
        public int? ClaimNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? PaymentTerm { get; set; }
        public int? Payer { get; set; }
        public string PaymentTermName { get; set; }
        public string PayerName { get; set; }
        public int? ReferenceNumber { get; set; }
    }

}

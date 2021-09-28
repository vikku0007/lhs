using System;
using System.Collections.Generic;

using System.Text;

namespace LHSAPI.Application.Client.Models
{
  public class ClientFundingInfo 
  {

    public int Id { get; set; }

    public int ClientId { get; set; }

        public int? FundType { get; set; }

        public string RefNumber { get; set; }

        public string Other { get; set; }
        public string FundTypeName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public double Amount { get; set; }
        public int? ClaimNumber { get; set; }
        public List<ClientFunding> ClientFunding { get; set; }

  }

}

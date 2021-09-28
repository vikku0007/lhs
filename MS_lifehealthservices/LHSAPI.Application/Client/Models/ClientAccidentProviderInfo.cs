using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Application.Client.Models
{
  public  class ClientAccidentProviderInfo
    {
        
        public int Id { get; set; }
        public int ClientId { get; set; }

        public string ReportCompletedBy { get; set; }

        public string ProviderName { get; set; }

        public string ProviderregistrationId { get; set; }

        public string ProviderABN { get; set; }

        public string OutletName { get; set; }

        public string Registrationgroup { get; set; }

        public int State { get; set; }
        public string StateName { get; set; }

    }
}

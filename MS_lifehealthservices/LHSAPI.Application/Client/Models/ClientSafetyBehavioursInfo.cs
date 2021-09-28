using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Application.Client.Models
{
  public  class ClientSafetyBehavioursInfo 
    {
        
        public int Id { get; set; }
        public int ClientId { get; set; }
        public bool? IsHistoryFalls { get; set; }

        public string HistoryFallsDetails { get; set; }

        public bool? IsAbsconding { get; set; }

        public string AbscondingHistory { get; set; }

        public bool? IsBSP { get; set; }

        public string BSPDetails { get; set; }
    }
}

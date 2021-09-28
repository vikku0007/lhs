using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Domain.Entities
{
  public  class ClientSafetyBehavioursInfo : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

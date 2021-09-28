using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Application.Client.Models
{
   public class ClientBehaviourofConcern 
    {
        
        public int Id { get; set; }
        public int ClientId { get; set; }
        public bool? IsBehaviourConcern { get; set; }

        public bool? IsSelfInjury { get; set; }

        public bool? IsHitting { get; set; }

        public bool? IsPullingHair { get; set; }

        public bool? IsHeadButting { get; set; }

        public bool? IsBanging { get; set; }

        public bool? IsKicking { get; set; }

        public bool? IsScreaming { get; set; }

        public bool? IsBiting { get; set; }

        public bool? IsThrowing { get; set; }

        public bool? IsObsessiveBehaviour { get; set; }

        public bool? IsOther { get; set; }
        public bool? IsPinching { get; set; }
        public string BehaviourConcernDetails { get; set; }
    }
}

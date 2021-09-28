using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddClientBehaviourConcern
{
  public class AddClientBehaviourConcernCommand : IRequest<ApiResponse>
  {
        public int Id { get; set; }
        public int ClientId { get; set; }

        public bool? IsHistoryFalls { get; set; }

        public string HistoryFallsDetails { get; set; }

        public bool? IsAbsconding { get; set; }

        public string AbscondingHistory { get; set; }

        public bool? IsBSP { get; set; }

        public string BSPDetails { get; set; }
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

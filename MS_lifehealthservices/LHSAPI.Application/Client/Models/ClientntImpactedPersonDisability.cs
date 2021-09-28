using LHSAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace LHSAPI.Application.Client.Models
{
   public class ClientntImpactedPersonDisability
    {
        public IncidentImpactedPerson IncidentImpactedPerson { get; set; }
        public List<PrimaryDisabilityModel> IncidentPrimaryDisability { get; set; }
        public List<PrimaryDisabilityModel> IncidentOtherDisability { get; set; }
        public List<PrimaryDisabilityModel> IncidentConcernBehaviour { get; set; }
        public List<PrimaryDisabilityModel> ClientIncidentCommunication { get; set; }
        
    }

}
